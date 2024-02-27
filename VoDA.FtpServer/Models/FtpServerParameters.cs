using VoDA.FtpServer.Contexts;

namespace VoDA.FtpServer.Models
{
    internal class FtpServerParameters
    {
        public FtpServerParameters(FtpServerOptions serverOptions, AuthorizationOptionsContext serverAuthorization,
            FileSystemOptionsContext serverFileSystemOptions, CertificateOptionsContext serverCertificate,
            FtpServerLogOptions serverLogOptions, AccessControlOptionsContext serverAccessControl)
        {
            this.serverOptions = serverOptions;
            this.serverAuthorization = serverAuthorization;
            this.serverFileSystemOptions = serverFileSystemOptions;
            this.serverCertificate = serverCertificate;
            this.serverLogOptions = serverLogOptions;
            this.serverAccessControl = serverAccessControl;
        }

        public FtpServerOptions serverOptions { get; }
        public AuthorizationOptionsContext serverAuthorization { get; }
        public FileSystemOptionsContext serverFileSystemOptions { get; }
        public CertificateOptionsContext serverCertificate { get; }
        public FtpServerLogOptions serverLogOptions { get; }
        public AccessControlOptionsContext serverAccessControl { get; }

        public static implicit operator FtpClientParameters(FtpServerParameters parameters)
        {
            return new FtpClientParameters(parameters.serverOptions, parameters.serverAuthorization,
                parameters.serverFileSystemOptions,
                parameters.serverCertificate);
        }
    }
}