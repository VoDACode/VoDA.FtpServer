namespace VoDA.FtpServer.Interfaces
{
    public enum LogLevel { None = -1, Information, Debug }
    public interface IFtpServerLogOptions
    {
        public LogLevel Level { get; set; }
    }
}
