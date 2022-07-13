using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Models
{
    internal class FtpServerLogOptions : IFtpServerLogOptions
    {
        public LogLevel Leve { get; set; } = LogLevel.Information;
    }
}
