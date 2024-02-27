namespace VoDA.FtpServer.Contexts
{
    public abstract class CertificateOptionsContext
    {
        /// <summary>
        ///     The path to a public certificate.
        /// </summary>
        public abstract string CertificatePath { get; set; }

        /// <summary>
        ///     The path to a private certificate.
        /// </summary>
        public abstract string CertificateKey { get; set; }
    }
}