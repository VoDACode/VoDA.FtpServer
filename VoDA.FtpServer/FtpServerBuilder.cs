using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

using VoDA.FtpServer.Models;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Contexts;
using VoDA.FtpServer.Commands;

namespace VoDA.FtpServer
{
    public class FtpServerBuilder
    {
#nullable disable
        private readonly FtpServerOptions _serverOptions = new();
        private AuthorizationOptionsContext _serverAuthorization = new FtpServerAuthorizationOptions();
        private CertificateOptionsContext _serverCertificate;
        private FileSystemOptionsContext _serverFileSystemOptions;
        private AccessControlOptionsContext _serverAccessControl = new FtpServerAccessControlOptions();
        private readonly FtpServerLogOptions _serverLogOptions = new();
        private FtpServer _server;
#nullable enable

        /// <summary>
        /// Configures the operation of the log system.
		/// </summary>
		/// <returns>A builder object.</returns>
        public FtpServerBuilder Log(Action<IFtpServerLogOptions> config)
        {
            config.Invoke(_serverLogOptions);
            return this;
        }

        /// <summary>
        /// Configures the operation of the server listener.
        /// </summary>
        /// <returns>A builder object.</returns>
        public FtpServerBuilder ListenerSettings(Action<IFtpServerOptions> config)
        {
            RunAndValid(config, _serverOptions);
            return this;
        }

        /// <summary>
        /// Configures the operation of the authorization.
        /// </summary>
        /// <returns>A builder object.</returns>
        // ReSharper disable once UnusedMember.Global
        public FtpServerBuilder Authorization(Action<IFtpServerAuthorizationOptions> config)
        {
            var data = new FtpServerAuthorizationOptions();
            config.Invoke(data);
            data.Valid();
            _serverAuthorization = data;
            return this;
        }

        /// <summary>
        /// Configures the operation of the authorization.
        /// </summary>
        /// <typeparam name="T">Must inherit from <see cref="AuthorizationOptionsContext"/> and have a <c>new()</c> implementation</typeparam>
        /// <returns>A builder object.</returns>
        public FtpServerBuilder Authorization<T>() where T : AuthorizationOptionsContext, new()
        {
            _serverAuthorization = new T();
            return this;
        }

        /// <summary>
        /// Configures work with certificates.
        /// </summary>
        /// <returns>A builder object.</returns>
        public FtpServerBuilder Certificate(Action<IFtpServerCertificateOptions> config)
        {
            var data = new FtpServerCertificateOptions();
            config.Invoke(data);
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
                var ecdSa = ECDsa.Create();
                var req = new CertificateRequest("cn=VoDA.FTP", ecdSa, HashAlgorithmName.SHA256);
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

        /// <summary>
        /// Configures work with certificates.
        /// </summary>
        /// <typeparam name="T">Must inherit from <see cref="CertificateOptionsContext"/> and have a <c>new()</c> implementation</typeparam>
        /// <returns>A builder object.</returns>
        // ReSharper disable once UnusedMember.Global
        public FtpServerBuilder Certificate<T>() where T : CertificateOptionsContext, new()
        {
            _serverCertificate = new T();
            return this;
        }

        /// <summary>
        /// Configures work with the file system.
        /// </summary>
        /// <returns>A builder object.</returns>
        public FtpServerBuilder FileSystem(Action<IFtpServerFileSystemOptions> config)
        {
            var data = new FtpServerFileSystemOptions();
            config.Invoke(data);
            data.Valid();
            _serverFileSystemOptions = data;
            return this;
        }

        /// <summary>
        /// Configures work with the file system.
        /// </summary>
        /// <typeparam name="T">Must inherit from <see cref="FileSystemOptionsContext"/> and have a <c>new()</c> implementation</typeparam>
        /// <returns>A builder object.</returns>
        public FtpServerBuilder FileSystem<T>() where T : FileSystemOptionsContext, new()
        {
            _serverFileSystemOptions = new T();
            return this;
        }

        /// <summary>
        /// Configures access to the server.
        /// </summary>
        /// <returns>A builder object.</returns>
        // ReSharper disable once UnusedMember.Global
        public FtpServerBuilder AccessControl(Action<IFtpServerAccessControlOptions> config)
        {
            var data = new FtpServerAccessControlOptions();
            config.Invoke(data);
            _serverAccessControl = data;
            return this;
        }

        /// <summary>
        /// Configures access to the server.
        /// </summary>
        /// <typeparam name="T">Must inherit from <see cref="AccessControlOptionsContext"/> and have a <c>new()</c> implementation</typeparam>
        /// <returns>A builder object.</returns>
        // ReSharper disable once UnusedMember.Global
        public FtpServerBuilder AccessControl<T>() where T : AccessControlOptionsContext, new()
        {
            _serverAccessControl = new T();
            return this;
        }


        /// <summary>
        /// Adds a custom command.
        /// </summary>
        /// <returns>A builder object.</returns>
        public FtpServerBuilder AddCommand<T>() where T : BaseCommand
        {
            FtpCommandHandler.Instance.Add<T>();
            return this;
        }

        /// <summary>
        /// Creates a server instance.
        /// </summary>
        /// <returns>The interface <see cref="IFtpServerControl"/> for managing the server.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IFtpServerControl Build()
        {
            if (_serverFileSystemOptions == null)
                throw new ArgumentNullException(nameof(_serverFileSystemOptions),"The algorithm for processing requests to work with the file system is not specified!");
            if (_serverCertificate == null)
                throw new ArgumentNullException(nameof(_serverCertificate), "Security certificate file names are not specified!");
            _server = new FtpServer(new FtpServerParameters(_serverOptions, _serverAuthorization, _serverFileSystemOptions, _serverCertificate, _serverLogOptions, _serverAccessControl));
            return _server;
        }

        private static void RunAndValid<TAction, TType>(Action<TAction> action, TType type) 
            where TType : IValidConfig, TAction
        {
            action.Invoke(type);
            type.Valid();
        }
    }
}
