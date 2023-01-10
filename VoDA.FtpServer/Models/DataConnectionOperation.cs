using System;
using System.Net.Sockets;

using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Models
{
    public class DataConnectionOperation
    {
        public Func<NetworkStream, string, IFtpResult> Func { get; }
        public string Arguments { get; }
        public DataConnectionOperation(Func<NetworkStream, string, IFtpResult> func, string arguments)
        {
            Func = func;
            Arguments = arguments;
        }
    }
}
