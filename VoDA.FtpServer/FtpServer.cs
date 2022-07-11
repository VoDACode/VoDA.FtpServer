using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer
{
    public class FtpServer : IFtpServerControl
    {
        private TcpListener _serverSocket;
        private FtpServerOptions _options = new FtpServerOptions();
        private FtpServerAuthorization _ftpAuthorization = new FtpServerAuthorization();
        private FtpServerFileSystemOptions _fileSystemOptions = new FtpServerFileSystemOptions();
        private bool _isEnable = false;
        private Task _handlerTask;
        private CancellationToken cancellation;
        private List<FtpClient> ftpClients = new List<FtpClient>();

        public FtpServer(Action<IFtpServerOptions> options,
            Action<IFtpServerFileSystemOptions> fileSystem,
            Action<IFtpServerAuthorization>? authorization = default)
        {
            options.Invoke(_options);
            if (!string.IsNullOrWhiteSpace(_options.Certificate.CertificatePath) &&
                !string.IsNullOrWhiteSpace(_options.Certificate.CertificateKey))
            {
                _options.Certificate.CertificatePath = Path.Join(
                        Path.GetDirectoryName(Path.GetFullPath(_options.Certificate.CertificatePath)),
                        Path.GetFileName(_options.Certificate.CertificatePath)
                    );
                _options.Certificate.CertificateKey = Path.Join(
                        Path.GetDirectoryName(Path.GetFullPath(_options.Certificate.CertificateKey)),
                        Path.GetFileName(_options.Certificate.CertificateKey)
                    );
            }
            if (!string.IsNullOrWhiteSpace(_options.Certificate.CertificatePath) &&
                !File.Exists(_options.Certificate.CertificatePath) && 
                !string.IsNullOrWhiteSpace(_options.Certificate.CertificateKey) &&
                !File.Exists(_options.Certificate.CertificateKey))
            {
                // https://stackoverflow.com/a/52535184
                var ecdsa = ECDsa.Create();
                var req = new CertificateRequest("cn=VoDA.FTP", ecdsa, HashAlgorithmName.SHA256);
                var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));
                File.WriteAllBytes(_options.Certificate.CertificateKey, cert.Export(X509ContentType.Pfx, "637925437145433542"));

                File.WriteAllText(_options.Certificate.CertificatePath,
                    "-----BEGIN CERTIFICATE-----\r\n"
                    + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
                    + "\r\n-----END CERTIFICATE-----");
            }
            fileSystem.Invoke(_fileSystemOptions);
            authorization?.Invoke(_ftpAuthorization);
            _serverSocket = new TcpListener(_options.ServerIp, _options.Port);
            var logger = new LoggerConfiguration();
            if (_options.IsEnableLog)
                logger.WriteTo.Console();
            Log.Logger = logger.CreateLogger();
        }

        public Task StartAsync(CancellationToken token)
        {
            if (_isEnable)
                throw new Exception("Server is runing.");
            _isEnable = true;
            cancellation = token;
            _serverSocket.Start();
            Log.Information($"Server is running (ftp://localhost:{_options.Port}/)");
            if (_options.MaxConnections > 0)
            {
                Log.Information($"Enabled limited connections mode.");
                Log.Information($"Max count connections = {_options.MaxConnections}");
            }
            if (_options.ServerIp != System.Net.IPAddress.Any)
            {
                Log.Information($"Enabled filter connections mode.");
                Log.Information($"Waiting connection from {_options.ServerIp}");
            }
            return handler(token);
        }

        public Task StopAsync()
        {
            if (!_isEnable)
                throw new Exception("Server isn`t runing.");
            _isEnable = false;
            return Task.CompletedTask;
        }

        private Task handler(CancellationToken token)
        {
            _handlerTask = Task.Run(() =>
            {
                while (!token.IsCancellationRequested && _isEnable)
                {
                    var tcp = _serverSocket.AcceptTcpClient();
                    if (_options.MaxConnections > 0 && ftpClients.Count >= _options.MaxConnections)
                    {
                        StreamWriter stream = new StreamWriter(tcp.GetStream());
                        stream.WriteLine("221 The server is full!");
                        stream.Flush();
                        stream.Close();
                        tcp.Close();
                        stream = null;
                        continue;
                    }
                    var client = new FtpClient(tcp);
                    ftpClients.Add(client);
                    client.OnEndProcessing += Client_OnEndProcessing;
                    client.HandleClient(_options, _ftpAuthorization, _fileSystemOptions);
                }
                _serverSocket.Stop();
            });
            return _handlerTask;
        }

        private void Client_OnEndProcessing(FtpClient client)
        {
            ftpClients.Remove(client);
        }
    }
}
