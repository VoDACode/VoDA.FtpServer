namespace VoDA.FtpServer.Contexts
{
    public abstract class CertificateOptionsContext
    {
        public abstract string CertificatePath { get; set; }
        public abstract string CertificateKey { get; set; }
    }
}
