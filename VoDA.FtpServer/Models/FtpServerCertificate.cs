using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Models
{
    internal class FtpServerCertificate : IFtpServerCertificate
    {
        public string? CertificatePath { get; set; }
        public string? CertificateKey { get; set; }
    }
}
