using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Models
{
    internal class FtpServerLogOptions : IFtpServerLogOptions
    {
        public LogLevel Level { get; set; } = LogLevel.Information;
    }
}