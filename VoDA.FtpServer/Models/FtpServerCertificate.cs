using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Models
{
    internal class FtpServerCertificateOptions : IFtpServerCertificateOptions
    {
        public string? CertificatePath { get; set; }
        public string? CertificateKey { get; set; }
    }
}
