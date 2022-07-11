using Serilog;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using VoDA.FtpServer.Enums;
using VoDA.FtpServer.Extensions;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Responses;

namespace VoDA.FtpServer.Models
{
    internal class FtpClient : IFtpClient
    {
        public TcpClient TcpSocket { get; set; }
        private TcpClient _dataClient { get; set; }
        private NetworkStream _stream { get; }
        public StreamReader StreamReader { get; set; }
        public StreamWriter StreamWriter { get; set; }
        public ConnectionType ConnectionType { get; set; } = ConnectionType.Active;
        public FileStructureType FileStructureType { get; set; } = FileStructureType.File;
        public IPEndPoint RemoteEndpoint { get; set; }
        public IPEndPoint DataEndpoint { get; set; }
        public string RenameFrom { get; set; }
        public int BufferSize { get; set; } = 4096;
        private long _lastPoint { get; set; } = 0;
        private DataConnectionOperation _lastDataConnection { get; set; }

        private SslStream _sslStream { get; set; }
        private X509Certificate _certificate { get; set; }

        public TcpListener PassiveListener { get; set; }
        private FtpServerAuthorization _authorization;
        private FtpServerFileSystemOptions _fileSystem;
        private FtpServerOptions _serverOptions;
        private FtpServerCertificate _serverCertificate;
        private Task _handleTask;
        private string _root;
        private CancellationTokenSource _cancellationTokenSource;

        public TransferType TransferType { get; set; }
        public string DataConnectionType { get; set; }
        public bool IsAuthorized { get; set; }
        public string? Username { get; set; }
        public string Root
        {
            set => _root = value;
            get => string.IsNullOrWhiteSpace(_root) ? Path.DirectorySeparatorChar.ToString() : _root;
        }
        public Task HandleTask => _handleTask;

        public event Action<FtpClient> OnEndProcessing;

        public FtpClient(TcpClient tcpSocket)
        {
            TcpSocket = tcpSocket;
            _stream = tcpSocket.GetStream();
            StreamReader = new StreamReader(_stream);
            StreamWriter = new StreamWriter(_stream);
        }

        public void HandleClient(FtpServerOptions serverOptions, FtpServerAuthorization authorization, FtpServerFileSystemOptions fileSystem, FtpServerCertificate serverCertificate)
        {
            _serverCertificate = serverCertificate;
            _serverOptions = serverOptions;
            _authorization = authorization;
            _fileSystem = fileSystem;
            _handleTask = Task.Run(_handleClient);
        }

        private async void _handleClient()
        {
            StreamWriter.WriteLine("220 Service Ready.");
            StreamWriter.Flush();
            RemoteEndpoint = (IPEndPoint)TcpSocket.Client.RemoteEndPoint;
            Log.Information($"[{RemoteEndpoint}] [S -> C]: 220 Service Ready.");
            string? line = string.Empty;
            try
            {
                while (TcpSocket.Connected && !string.IsNullOrEmpty(line = await StreamReader.ReadLineAsync()))
                {
                    var command = new FtpCommand(line);
                    Log.Information($"[{RemoteEndpoint}][{Username}] [S <- C]: {command}");
                    var response = await FtpCommandHandler.Instance.HandleCommand(command, this, _authorization, _fileSystem, _serverOptions);
                    Log.Information($"[{RemoteEndpoint}][{Username}] [S -> C]: {response.Code} {response.Text}");
                    if (TcpSocket == null || !TcpSocket.Connected)
                        break;
                    else
                    {
                        StreamWriter.WriteLine($"{response.Code} {response.Text}");
                        StreamWriter.Flush();
                        if (response.Code == 221)
                            break;
                        if (command.Command == "AUTH" && !string.IsNullOrWhiteSpace(_serverCertificate.CertificatePath) 
                                                      && !string.IsNullOrWhiteSpace(_serverCertificate.CertificateKey))
                        {
                            _certificate = new X509Certificate2(_serverCertificate.CertificateKey, "637925437145433542");
                            _sslStream = new SslStream(_stream, false);
                            _sslStream.AuthenticateAsServer(_certificate);
                            StreamReader = new StreamReader(_sslStream);
                            StreamWriter = new StreamWriter(_sslStream);
                        }
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
            if(_cancellationTokenSource == null || _cancellationTokenSource.Token.IsCancellationRequested)
                return;
            _cancellationTokenSource.Cancel();
        }
        public void RestoreLastCommand(long len)
        {
            if (_lastDataConnection == null || !_cancellationTokenSource.IsCancellationRequested)
                return;
            _lastPoint = len;
            SetupDataConnectionOperation(_lastDataConnection);
        }

        public void SetupDataConnectionOperation(DataConnectionOperation dataConnection)
        {
            _lastDataConnection = dataConnection;
            if (ConnectionType == ConnectionType.Active)
            {
                _dataClient = new TcpClient(DataEndpoint.AddressFamily);
                _dataClient.BeginConnect(DataEndpoint.Address, DataEndpoint.Port, eventDataConnectionOperation, dataConnection);
            }
            else
            {
                PassiveListener.BeginAcceptTcpClient(eventDataConnectionOperation, dataConnection);
            }
        }

        public IFtpResult RetrieveOperation(NetworkStream stream, string path)
        {
            using (FileStream fs = _fileSystem.Download(this, path))
            {
                _cancellationTokenSource = new CancellationTokenSource();
                _lastPoint = fs.CopyToStream(stream, BufferSize, TransferType, _cancellationTokenSource.Token, _lastPoint);
                if (_cancellationTokenSource.IsCancellationRequested)
                    _lastPoint = 0;
            }
            return new CustomResponse("Closing data connection, file transfer successful", 226);
        }

        public IFtpResult StoreOperation(NetworkStream stream, string path)
        {
            using (FileStream fs = _fileSystem.Upload(this, path))
            {
                _cancellationTokenSource = new CancellationTokenSource();
                _lastPoint = stream.CopyToStream(fs, BufferSize, TransferType, _cancellationTokenSource.Token, _lastPoint);
                if (_cancellationTokenSource.IsCancellationRequested)
                    _lastPoint = 0;
            }
            return new CustomResponse("Closing data connection, file transfer successful", 226);
        }

        public IFtpResult AppendOperation(NetworkStream stream, string path)
        {
            using (FileStream fs = _fileSystem.Append(this, path))
            {
                _cancellationTokenSource = new CancellationTokenSource();
                _lastPoint = stream.CopyToStream(fs, BufferSize, TransferType, _cancellationTokenSource.Token, _lastPoint);
                if (_cancellationTokenSource.IsCancellationRequested)
                    _lastPoint = 0;
            }
            return new CustomResponse("Closing data connection, file transfer successful", 226);
        }

        public IFtpResult ListOperation(NetworkStream stream, string path)
        {
            StreamWriter sw = new StreamWriter(stream);
            var list = _fileSystem.List(this, path);
            foreach (var item in list.Item1)
            {
                string date = item.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                            item.LastWriteTime.ToString("MMM dd  yyyy") :
                            item.LastWriteTime.ToString("MMM dd HH:mm");
                sw.WriteLine(string.Format("drwxr-xr-x    2 2003     2003     {0,8} {1} {2}", "4096", date, item.Name));
                sw.Flush();
            }
            foreach (var item in list.Item2)
            {
                string date = item.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                            item.LastWriteTime.ToString("MMM dd  yyyy") :
                            item.LastWriteTime.ToString("MMM dd HH:mm");
                sw.WriteLine(string.Format("-rw-r--r--    2 2003     2003     {0,8} {1} {2}", item.Length, date, item.Name));
                sw.Flush();
            }
            return new CustomResponse("Closing data connection, file transfer successful", 226);
        }

        private void eventDataConnectionOperation(IAsyncResult result)
        {
            if (ConnectionType == ConnectionType.Active)
                _dataClient.EndConnect(result);
            else
                _dataClient = PassiveListener.EndAcceptTcpClient(result);

            DataConnectionOperation options = result.AsyncState as DataConnectionOperation;
            IFtpResult response;
            using (NetworkStream dataStream = _dataClient.GetStream())
            {
                response = options.Func(dataStream, options.Arguments);
            }
            _dataClient.Close();
            _dataClient = null;
            StreamWriter.WriteLine($"{response.Code} {response.Text}");
            StreamWriter.Flush();
            Log.Information($"[{RemoteEndpoint}][{Username}] [S -> C]: {response.Code} {response.Text}");
        }


    }
}
