using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

using VoDA.FtpServer.Models;
using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer
{
    public class FtpServerBuilder
    {
        private FtpServerOptions _serverOptions;
        private FtpServerAuthorization _serverAuthorization;
        private FtpServerCertificate _serverCertificate;
        private FtpServerFileSystemOptions _serverFileSystemOptions;
        private FtpServer _server;

        public FtpServerBuilder()
        {
            _serverOptions = new FtpServerOptions();
            _serverAuthorization = new FtpServerAuthorization();
            _serverCertificate = new FtpServerCertificate();
            _serverFileSystemOptions = new FtpServerFileSystemOptions();
        }

        public FtpServerBuilder ListenerSettings(Action<IFtpServerOptions> config)
        {
            runAndValid(config, _serverOptions);
            return this;
        }

        public FtpServerBuilder Authorization(Action<IFtpServerAuthorization> config)
        {
            runAndValid(config, _serverAuthorization);
            return this;
        }

        public FtpServerBuilder Certificate(Action<IFtpServerCertificate> config)
        {
            config?.Invoke(_serverCertificate);
            if (!string.IsNullOrWhiteSpace(_serverCertificate.CertificatePath) &&
                !string.IsNullOrWhiteSpace(_serverCertificate.CertificateKey))
            {
                _serverCertificate.CertificatePath = Path.Join(
                        Path.GetDirectoryName(Path.GetFullPath(_serverCertificate.CertificatePath)),
                        Path.GetFileName(_serverCertificate.CertificatePath)
                    );
                _serverCertificate.CertificateKey = Path.Join(
                        Path.GetDirectoryName(Path.GetFullPath(_serverCertificate.CertificateKey)),
                        Path.GetFileName(_serverCertificate.CertificateKey)
                    );
            }
            if (!string.IsNullOrWhiteSpace(_serverCertificate.CertificatePath) &&
                !File.Exists(_serverCertificate.CertificatePath) &&
                !string.IsNullOrWhiteSpace(_serverCertificate.CertificateKey) &&
                !File.Exists(_serverCertificate.CertificateKey))
            {
                // https://stackoverflow.com/a/52535184
                var ecdsa = ECDsa.Create();
                var req = new CertificateRequest("cn=VoDA.FTP", ecdsa, HashAlgorithmName.SHA256);
                var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));
                File.WriteAllBytes(_serverCertificate.CertificateKey, cert.Export(X509ContentType.Pfx, "637925437145433542"));

                File.WriteAllText(_serverCertificate.CertificatePath,
                    "-----BEGIN CERTIFICATE-----\r\n"
                    + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
                    + "\r\n-----END CERTIFICATE-----");
            }
            return this;
        }

        public FtpServerBuilder FileSystem(Action<IFtpServerFileSystemOptions> config)
        {
            runAndValid(config, _serverFileSystemOptions);
            return this;
        }

        public IFtpServerControl Build()
        {
            _server = new FtpServer(_serverOptions, _serverAuthorization, _serverFileSystemOptions, _serverCertificate);
            return _server;
        }

        private void runAndValid<T, I>(Action<T> action, I type) where I : IValidConfig, T
        {
            action?.Invoke(type);
            type.Valid();
        }
    }
}
