using VoDA.FtpServer.Contexts;

namespace VoDA.FtpServer.Models
{
    public class FtpClientParameters
    {
        public FtpServerOptions ServerOptions { get; }
        public AuthorizationOptionsContext AuthorizationOptions { get; }
        public FileSystemOptionsContext FileSystemOptions { get; }
        public CertificateOptionsContext CertificateOptions { get; }
        public FtpClientParameters(FtpServerOptions serverOptions, AuthorizationOptionsContext authorizationOptions, FileSystemOptionsContext fileSystemOptions, CertificateOptionsContext certificateOptions)
        {
            ServerOptions = serverOptions;
            AuthorizationOptions = authorizationOptions;
            FileSystemOptions = fileSystemOptions;
            CertificateOptions = certificateOptions;
        }
    }
}
