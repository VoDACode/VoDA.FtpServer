namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpServerCertificate
    {
        public string? CertificatePath { get; set; }
        public string? CertificateKey { get; set; }
    }
}
