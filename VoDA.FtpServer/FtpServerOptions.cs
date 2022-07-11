using System.Net;

using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer
{
    internal class FtpServerOptions : IFtpServerOptions
    {
        public int Port { get; set; }
        public int MaxConnections { get; set; }
        public IPAddress ServerIp { get; set; } = IPAddress.Any;

        public IFtpServerCertificate Certificate { get; } = new FtpServerCertificate();
        public bool IsEnableLog { get; set; } = true;
    }
}
