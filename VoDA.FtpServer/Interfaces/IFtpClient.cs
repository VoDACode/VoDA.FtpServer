using System.Net;

namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpClient
    {
        /// <summary>
        ///     User name.
        /// </summary>
        public string Username { get; }

        /// <summary>
        ///     User connection address.
        /// </summary>
        public IPEndPoint? RemoteEndpoint { get; }

        /// <summary>
        ///     Indicates whether the user is authorized
        /// </summary>
        public bool IsAuthorized { get; }

        /// <summary>
        ///     Terminates the session.
        /// </summary>
        public void Kik();
    }
}