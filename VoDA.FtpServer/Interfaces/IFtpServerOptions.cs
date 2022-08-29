using System.Net;

namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpServerOptions
    {
        /// <summary>
        /// The port on which the server will work. Default 21.
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Maximum number of connections. Default is infinity (-1).
        /// </summary>
        public int MaxConnections { get; set; }
        public IPAddress ServerIp { get; set; }
    }
}
