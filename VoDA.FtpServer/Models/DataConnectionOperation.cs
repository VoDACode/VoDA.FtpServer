using System;
using System.Net.Sockets;
using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Models
{
    public class DataConnectionOperation
    {
        public DataConnectionOperation(Func<NetworkStream, string, IFtpResult> func, string arguments)
        {
            Func = func;
            Arguments = arguments;
        }

        public Func<NetworkStream, string, IFtpResult> Func { get; }
        public string Arguments { get; }
    }
}