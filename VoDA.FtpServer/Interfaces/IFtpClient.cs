using System.Net;

namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpClient
    {
        public string Username { get; }
        public IPEndPoint RemoteEndpoint { get; }
        public bool IsAuthorized { get; }
        public void Kik();
    }
}
