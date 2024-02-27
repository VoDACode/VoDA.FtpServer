using System;
using System.Net;
using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer
{
    public class FtpServerOptions : IFtpServerOptions, IValidConfig
    {
        public int Port { get; set; } = 21;
        public int MaxConnections { get; set; }
        public IPAddress ServerIp { get; set; } = IPAddress.Any;

        public void Valid()
        {
            if (Port is <= 0 or >= 65536)
                throw new ArgumentOutOfRangeException(nameof(Port));
            if (MaxConnections < 0)
                throw new ArgumentOutOfRangeException(nameof(MaxConnections));
        }
    }
}