namespace VoDA.FtpServer.Interfaces
{
    public enum LogLevel
    {
        None = -1,
        Information,
        Debug
    }

    public interface IFtpServerLogOptions
    {
        /// <summary>
        ///     Specifies the log level.
        /// </summary>
        public LogLevel Level { get; set; }
    }
}