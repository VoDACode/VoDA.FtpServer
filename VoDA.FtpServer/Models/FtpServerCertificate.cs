using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Models
{
    internal class FtpServerCertificateOptions : Contexts.CertificateOptionsContext, IFtpServerCertificateOptions
    {
        public override string CertificatePath { get; set; } = string.Empty;
        public override string CertificateKey { get; set; } = string.Empty;
    }
}
