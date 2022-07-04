using Serilog;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

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
        private StreamReader _streamReader { get; set; }
        private StreamWriter _streamWriter { get; set; }
        public ConnectionType ConnectionType { get; set; } = ConnectionType.Active;
        public FileStructureType FileStructureType { get; set; } = FileStructureType.File;
        public IPEndPoint RemoteEndpoint { get; set; }
        public IPEndPoint DataEndpoint { get; set; }
        public string RenameFrom { get; set; }

        private SslStream _sslStream { get; set; }
        private X509Certificate _certificate { get; set; }

        public TcpListener PassiveListener { get; set; }
        private FtpServerAuthorization _authorization;
        private FtpServerFileSystemOptions _fileSystem;
        private FtpServerOptions _serverOptions;
        private Task _handleTask;
        private string _root;

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

        public FtpClient(TcpClient tcpSocket)
        {
            TcpSocket = tcpSocket;
            _stream = tcpSocket.GetStream();
            _streamReader = new StreamReader(_stream);
            _streamWriter = new StreamWriter(_stream);
        }

        public void HandleClient(FtpServerOptions serverOptions, FtpServerAuthorization authorization, FtpServerFileSystemOptions fileSystem)
        {
            _serverOptions = serverOptions;
            _authorization = authorization;
            _fileSystem = fileSystem;
            _handleTask = Task.Run(_handleClient);
        }

        private async void _handleClient()
        {
            _streamWriter.WriteLine("220 Service Ready.");
            _streamWriter.Flush();
            RemoteEndpoint = (IPEndPoint)TcpSocket.Client.RemoteEndPoint;
            Log.Information($"[{RemoteEndpoint}] [S -> C]: 220 Service Ready.");
            string? line = string.Empty;
            try
            {
                while (!string.IsNullOrEmpty(line = _streamReader.ReadLine()))
                {
                    var command = new FtpCommand(line);
                    Log.Information($"[{RemoteEndpoint}] [S <- C]: {command}");
                    var response = await FtpCommandHandler.Instance.HandleCommand(command, this, _authorization, _fileSystem, _serverOptions);
                    Log.Information($"[{RemoteEndpoint}] [S -> C]: {response.Code} {response.Text}");
                    if (TcpSocket == null || !TcpSocket.Connected)
                        break;
                    else
                    {
                        _streamWriter.WriteLine($"{response.Code} {response.Text}");
                        _streamWriter.Flush();
                        if (response.Code == 221)
                            break;
                        if (command.Command == "AUTH" && !string.IsNullOrWhiteSpace(_serverOptions.Certificate.CertificatePath))
                        {
                            _certificate = new X509Certificate2(_serverOptions.Certificate.CertificateKey, "637925437145433542");
                            _sslStream = new SslStream(_stream, false);
                            _sslStream.AuthenticateAsServer(_certificate);
                            _streamReader = new StreamReader(_sslStream);
                            _streamWriter = new StreamWriter(_sslStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        public void SetupDataConnectionOperation(DataConnectionOperation dataConnection)
        {
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
            long len = 0;
            using (FileStream fs = _fileSystem.Download(this, path))
            {
                len = fs.CopyToStream(stream);
            }
            return new CustomResponse("Closing data connection, file transfer successful", 226);
        }

        public IFtpResult StoreOperation(NetworkStream stream, string path)
        {
            long len = 0;
            using (FileStream fs = _fileSystem.Upload(this, path))
            {
                stream.CopyToStream(fs);
            }
            return new CustomResponse("Closing data connection, file transfer successful", 226);
        }

        public IFtpResult AppendOperation(NetworkStream stream, string path)
        {
            long len = 0;
            using (FileStream fs = _fileSystem.Append(this, path))
            {
                stream.CopyToStream(fs);
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
            _streamWriter.WriteLine($"{response.Code} {response.Text}");
            _streamWriter.Flush();
        }
    }
}
