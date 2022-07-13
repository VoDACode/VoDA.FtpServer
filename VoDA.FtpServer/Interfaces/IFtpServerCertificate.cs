namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpServerCertificateOptions
    {
        public string? CertificatePath { get; set; }
        public string? CertificateKey { get; set; }
    }
}
