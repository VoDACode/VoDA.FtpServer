using Serilog;
using System;
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

        public FtpServer(Action<IFtpServerOptions> options, Action<IFtpServerFileSystemOptions> fileSystem, Action<IFtpServerAuthorization> authorization = default)
        {
            options.Invoke(_options);
            if (!string.IsNullOrWhiteSpace(_options.Certificate.CertificatePath))
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
                !File.Exists(_options.Certificate.CertificatePath))
            {
                var ecdsa = ECDsa.Create(); // generate asymmetric key pair
                var req = new CertificateRequest("cn=VoDA.FTP", ecdsa, HashAlgorithmName.SHA256);
                var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));

                // Create PFX (PKCS #12) with private key
                File.WriteAllBytes(_options.Certificate.CertificateKey, cert.Export(X509ContentType.Pfx, "637925437145433542"));

                // Create Base 64 encoded CER (public key only)
                File.WriteAllText(_options.Certificate.CertificatePath,
                    "-----BEGIN CERTIFICATE-----\r\n"
                    + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
                    + "\r\n-----END CERTIFICATE-----");
            }
            fileSystem.Invoke(_fileSystemOptions);
            authorization?.Invoke(_ftpAuthorization);
            _serverSocket = new TcpListener(_options.Address, _options.Port);
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
            return handler(token);
        }

        public async Task StopAsync()
        {
            if (!_isEnable)
                throw new Exception("Server isn`t runing.");
            _isEnable = false;
        }

        private Task handler(CancellationToken token)
        {
            _handlerTask = Task.Run(() =>
            {
                while (!token.IsCancellationRequested && _isEnable)
                {
                    var tcp = _serverSocket.AcceptTcpClient();
                    var client = new FtpClient(tcp);
                    client.HandleClient(_options, _ftpAuthorization, _fileSystemOptions);
                }
            });
            return _handlerTask;
        }
    }
}
