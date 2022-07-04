﻿using System.Net;

namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpServerOptions
    {
        public int Port { get; set; }
        public int MaxConnections { get; set; }
        public IPAddress Address { get; set; }
        public IFtpServerCertificate Certificate { get; }
        public bool IsEnableLog { get; set; }
    }
}
