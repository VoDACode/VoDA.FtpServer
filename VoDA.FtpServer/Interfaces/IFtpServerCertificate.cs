namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpServerCertificateOptions
    {
        /// <summary>
        ///     The path to a public certificate.
        /// </summary>
        public string CertificatePath { get; set; }

        /// <summary>
        ///     The path to a private certificate.
        /// </summary>
        public string CertificateKey { get; set; }
    }
}