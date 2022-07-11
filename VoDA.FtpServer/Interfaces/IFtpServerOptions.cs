using System.Net;

namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpServerOptions
    {
        public int Port { get; set; }
        public int MaxConnections { get; set; }
        public IPAddress ServerIp { get; set; }
        public bool IsEnableLog { get; set; }
    }
}
