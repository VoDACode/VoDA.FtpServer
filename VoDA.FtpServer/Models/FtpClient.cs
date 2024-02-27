using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using VoDA.FtpServer.Delegates;
using VoDA.FtpServer.Enums;
using VoDA.FtpServer.Extensions;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Responses;

namespace VoDA.FtpServer.Models
{
    public class FtpClient : IFtpClient
    {
        private CancellationTokenSource? _cancellationTokenSource;
        private string? _root;

        public FtpClient(TcpClient tcpSocket)
        {
            TcpSocket = tcpSocket;
            _stream = tcpSocket.GetStream();
            StreamReader = new StreamReader(_stream);
            StreamWriter = new StreamWriter(_stream);
        }

        public TcpClient? TcpSocket { get; set; }
        public TcpClient? DataClient { get; set; }
        private NetworkStream _stream { get; }
        public StreamReader? StreamReader { get; set; }
        public StreamWriter? StreamWriter { get; set; }
        public ConnectionType ConnectionType { get; set; } = ConnectionType.Active;
        public FileStructureType FileStructureType { get; set; } = FileStructureType.File;
        public TransferType TransferType { get; set; } = TransferType.Ascii;
        public IPEndPoint? DataEndpoint { get; set; }
        public string? RenameFrom { get; set; }
        public int BufferSize { get; set; } = 4096;
        private long _lastPoint { get; set; }
        private DataConnectionOperation? _lastDataConnection { get; set; }

        private SslStream? _sslStream { get; set; }
        private X509Certificate? _certificate { get; set; }

        public TcpListener? PassiveListener { get; set; }
#nullable disable
        private FtpClientParameters configParameters { get; set; }
#nullable enable
        public string Root
        {
            set => _root = value;
            get => string.IsNullOrWhiteSpace(_root) ? Path.DirectorySeparatorChar.ToString() : _root;
        }

        public Task? HandleTask { get; private set; }

        public IPEndPoint? RemoteEndpoint { get; set; }

        public bool IsAuthorized { get; set; }
        public string Username { get; set; } = string.Empty;

        public void Kik()
        {
            StopLastCommand();
            try
            {
                if (TcpSocket?.Connected == true)
                {
                    StreamWriter?.WriteLine("221 Bye!");
                    StreamWriter?.Flush();
                    StreamReader?.Close();
                    StreamWriter?.Close();
                    _sslStream?.Close();
                    TcpSocket?.Close();
                }
            }
            finally
            {
                StreamWriter = null;
                StreamReader = null;
                _sslStream = null;
                TcpSocket = null;
            }
        }

        public event Action<FtpClient>? OnEndProcessing;
        public event Action<FtpClient>? OnConnection;
        public event Action<FtpClient, string>? OnLog;
        public event Action<FtpClient, long, long>? OnUploadProgress;
        public event Action<FtpClient, long, long>? OnDownloadProgress;

        public event ClientDataProcessingStatusDelegate? OnStartUpload;
        public event ClientDataProcessingStatusDelegate? OnCompleteUpload;
        public event ClientDataProcessingStatusDelegate? OnStartDownload;
        public event ClientDataProcessingStatusDelegate? OnCompleteDownload;

        public void HandleClient(FtpClientParameters parameters)
        {
            configParameters = parameters;
            HandleTask = Task.Run(_handleClient);
        }

        private async void _handleClient()
        {
            StreamWriter?.WriteLine("220 Service Ready.");
            StreamWriter?.Flush();
            if (TcpSocket?.Client.RemoteEndPoint == null)
                throw new ArgumentNullException("TcpSocket.Client.RemoteEndPoint");
            RemoteEndpoint = (IPEndPoint)TcpSocket.Client.RemoteEndPoint;
            Log.Information($"[{RemoteEndpoint}] [S -> C]: 220 Service Ready.");
            if (!configParameters.AuthorizationOptions.UseAuthorization)
                OnConnection?.Invoke(this);
            var eventAlertConnection = !configParameters.AuthorizationOptions.UseAuthorization;
            var line = string.Empty;
            try
            {
                while (TcpSocket != null && TcpSocket.Connected && StreamReader != null &&
                       !string.IsNullOrEmpty(line = await StreamReader.ReadLineAsync()))
                {
                    var command = new FtpCommand(line);
                    logInfo(command.ToString(), true);
                    var response = await FtpCommandHandler.Instance.HandleCommand(command, this, configParameters);
                    logInfo(response.ToString(), false);
                    if (!eventAlertConnection && IsAuthorized)
                    {
                        OnConnection?.Invoke(this);
                        eventAlertConnection = true;
                    }

                    if (TcpSocket == null || !TcpSocket.Connected) break;

                    StreamWriter?.WriteLine(response.ToString());
                    StreamWriter?.Flush();
                    if (response.Code == 221)
                        break;
                    if (command.Command == "AUTH" && !string.IsNullOrWhiteSpace(configParameters.CertificateOptions
                                                      .CertificatePath)
                                                  && !string.IsNullOrWhiteSpace(configParameters.CertificateOptions
                                                      .CertificateKey))
                    {
                        _certificate = new X509Certificate2(configParameters.CertificateOptions.CertificateKey,
                            "637925437145433542");
                        _sslStream = new SslStream(_stream, false);
                        _sslStream.AuthenticateAsServer(_certificate);
                        StreamReader = new StreamReader(_sslStream);
                        StreamWriter = new StreamWriter(_sslStream);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                Log.Information($"Bye {Username}!");
                OnEndProcessing?.Invoke(this);
            }
        }

        public void StopLastCommand()
        {
            if (_cancellationTokenSource == null || _cancellationTokenSource.Token.IsCancellationRequested)
                return;
            _cancellationTokenSource.Cancel();
        }

        public void RestoreLastCommand(long len)
        {
            if (_lastDataConnection == null || !_cancellationTokenSource?.IsCancellationRequested == true)
                return;
            _lastPoint = len;
            SetupDataConnectionOperation(_lastDataConnection);
        }

        public void SetupDataConnectionOperation(DataConnectionOperation dataConnection)
        {
            _lastDataConnection = dataConnection;
            if (ConnectionType == ConnectionType.Active)
            {
                if (DataEndpoint == null)
                    throw new ArgumentNullException("DataEndpoint");
                DataClient = new TcpClient(DataEndpoint.AddressFamily);
                DataClient.BeginConnect(DataEndpoint.Address, DataEndpoint.Port, eventDataConnectionOperation,
                    dataConnection);
            }
            else
            {
                if (PassiveListener == null)
                    throw new ArgumentNullException("PassiveListener");
                PassiveListener.BeginAcceptTcpClient(eventDataConnectionOperation, dataConnection);
            }
        }

        public IFtpResult RetrieveOperation(NetworkStream stream, string path)
        {
            using (var fs = configParameters.FileSystemOptions.Download(this, path))
            {
                OnStartDownload?.Invoke(this, path);
                _cancellationTokenSource = new CancellationTokenSource();
                _lastPoint = fs.CopyToStream(stream, BufferSize, TransferType, _cancellationTokenSource.Token,
                    _lastPoint, (len, done) => { Task.Run(() => OnDownloadProgress?.Invoke(this, len, done)); });
                if (_cancellationTokenSource.IsCancellationRequested)
                    _lastPoint = 0;
            }

            OnCompleteDownload?.Invoke(this, path);
            return new CustomResponse("Closing data connection, file transfer successful", 226);
        }

        public IFtpResult StoreOperation(NetworkStream stream, string path)
        {
            using (var fs = configParameters.FileSystemOptions.Upload(this, path))
            {
                OnStartUpload?.Invoke(this, path);
                _cancellationTokenSource = new CancellationTokenSource();
                _lastPoint = stream.CopyToStream(fs, BufferSize, TransferType, _cancellationTokenSource.Token,
                    _lastPoint, (len, done) => { Task.Run(() => OnUploadProgress?.Invoke(this, len, done)); });
                if (_cancellationTokenSource.IsCancellationRequested)
                    _lastPoint = 0;
            }

            OnCompleteUpload?.Invoke(this, path);
            return new CustomResponse("Closing data connection, file transfer successful", 226);
        }

        public IFtpResult AppendOperation(NetworkStream stream, string path)
        {
            using (var fs = configParameters.FileSystemOptions.Append(this, path))
            {
                _cancellationTokenSource = new CancellationTokenSource();
                _lastPoint = stream.CopyToStream(fs, BufferSize, TransferType, _cancellationTokenSource.Token,
                    _lastPoint);
                if (_cancellationTokenSource.IsCancellationRequested)
                    _lastPoint = 0;
            }

            return new CustomResponse("Closing data connection, file transfer successful", 226);
        }

        public IFtpResult ListOperation(NetworkStream stream, string path)
        {
            var sw = new StreamWriter(stream, Encoding.ASCII);
            var list = configParameters.FileSystemOptions.List(this, path);
            foreach (var item in list.Item1)
            {
                var date = item.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180)
                    ? item.LastWriteTime.ToString("MMM dd  yyyy")
                    : item.LastWriteTime.ToString("MMM dd HH:mm");
                sw.WriteLine("drwxr-xr-x    2 2003     2003     {0,8} {1} {2}", "4096", date, item.Name);
                sw.Flush();
            }

            foreach (var item in list.Item2)
            {
                var date = item.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180)
                    ? item.LastWriteTime.ToString("MMM dd  yyyy")
                    : item.LastWriteTime.ToString("MMM dd HH:mm");
                sw.WriteLine("-rw-r--r--    2 2003     2003     {0,8} {1} {2}", item.Length, date, item.Name);
                sw.Flush();
            }

            return new CustomResponse("Closing data connection, file transfer successful", 226);
        }

        private void eventDataConnectionOperation(IAsyncResult result)
        {
            if (ConnectionType == ConnectionType.Active)
            {
                if (DataClient == null)
                    throw new ArgumentNullException("DataEndpoint");
                DataClient.EndConnect(result);
            }
            else
            {
                if (PassiveListener == null)
                    throw new ArgumentNullException("PassiveListener");
                DataClient = PassiveListener.EndAcceptTcpClient(result);
            }

            var options = result.AsyncState as DataConnectionOperation;
            if (options != null)
            {
                IFtpResult response;
                using (var dataStream = DataClient.GetStream())
                {
                    response = options.Func(dataStream, options.Arguments);
                }

                StreamWriter?.WriteLine($"{response.Code} {response.Text}");
                StreamWriter?.Flush();
                Log.Information($"[{RemoteEndpoint}][{Username}] [S -> C]: {response.Code} {response.Text}");
            }

            DataClient.Close();
            DataClient = null;
        }

        private void logInfo(string message, bool isCommand)
        {
            message = $"[{RemoteEndpoint}][{Username}] [S {(isCommand ? "<-" : "->")} C]: {message}";
            Log.Information(message);
            OnLog?.Invoke(this, message);
        }
    }
}