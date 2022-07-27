using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

using VoDA.FtpServer.Models;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Contexts;

namespace VoDA.FtpServer
{
    public class FtpServerBuilder
    {
        private FtpServerOptions _serverOptions;
        private AuthorizationOptionsContext _serverAuthorization;
        private CertificateOptionsContext _serverCertificate;
        private FileSystemOptionsContext _serverFileSystemOptions;
        private FtpServerLogOptions _serverLogOptions;
        private FtpServer? _server;

        public FtpServerBuilder()
        {
            _serverOptions = new FtpServerOptions();
            _serverLogOptions = new FtpServerLogOptions();
        }

        public FtpServerBuilder Log(Action<IFtpServerLogOptions> config)
        {
            config?.Invoke(_serverLogOptions);
            return this;
        }

        public FtpServerBuilder ListenerSettings(Action<IFtpServerOptions> config)
        {
            runAndValid(config, _serverOptions);
            return this;
        }

        public FtpServerBuilder Authorization(Action<IFtpServerAuthorizationOptions> config)
        {
            var data = new FtpServerAuthorizationOptions();
            config.Invoke(data);
            data.Valid();
            _serverAuthorization = data;
            return this;
        }

        public FtpServerBuilder Authorization<T>() where T : AuthorizationOptionsContext, new()
        {
            _serverAuthorization = new T();
            return this;
        }

        public FtpServerBuilder Certificate(Action<IFtpServerCertificateOptions> config)
        {
            var data = new FtpServerCertificateOptions();
            config?.Invoke(data);
            if (!string.IsNullOrWhiteSpace(data.CertificatePath) &&
                !string.IsNullOrWhiteSpace(data.CertificateKey))
            {
                data.CertificatePath = Path.Join(
                        Path.GetDirectoryName(Path.GetFullPath(data.CertificatePath)),
                        Path.GetFileName(data.CertificatePath)
                    );
                data.CertificateKey = Path.Join(
                        Path.GetDirectoryName(Path.GetFullPath(data.CertificateKey)),
                        Path.GetFileName(data.CertificateKey)
                    );
            }
            if (!string.IsNullOrWhiteSpace(data.CertificatePath) &&
                !File.Exists(data.CertificatePath) &&
                !string.IsNullOrWhiteSpace(data.CertificateKey) &&
                !File.Exists(data.CertificateKey))
            {
                // https://stackoverflow.com/a/52535184
                var ecdsa = ECDsa.Create();
                var req = new CertificateRequest("cn=VoDA.FTP", ecdsa, HashAlgorithmName.SHA256);
                var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));
                File.WriteAllBytes(data.CertificateKey, cert.Export(X509ContentType.Pfx, "637925437145433542"));

                File.WriteAllText(data.CertificatePath,
                    "-----BEGIN CERTIFICATE-----\r\n"
                    + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
                    + "\r\n-----END CERTIFICATE-----");
            }
            _serverCertificate = data;
            return this;
        }

        public FtpServerBuilder Certificate<T>() where T : CertificateOptionsContext, new()
        {
            _serverCertificate = new T();
            return this;
        }

        public FtpServerBuilder FileSystem(Action<IFtpServerFileSystemOptions> config)
        {
            var data = new FtpServerFileSystemOptions();
            config.Invoke(data);
            data.Valid();
            _serverFileSystemOptions = data;
            return this;
        }

        public FtpServerBuilder FileSystem<T>() where T : FileSystemOptionsContext, new()
        {
            _serverFileSystemOptions = new T();
            return this;
        }

        public IFtpServerControl Build()
        {
            _server = new FtpServer(_serverOptions, _serverAuthorization, _serverFileSystemOptions, _serverCertificate, _serverLogOptions);
            return _server;
        }

        private void runAndValid<T, I>(Action<T> action, I type) where I : IValidConfig, T
        {
            action?.Invoke(type);
            type.Valid();
        }
    }
}
