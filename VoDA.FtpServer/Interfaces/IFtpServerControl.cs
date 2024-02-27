using System.Threading;
using System.Threading.Tasks;

namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpServerControl
    {
        /// <summary>
        ///     Client session management interface.
        /// </summary>
        public ISessionsController Sessions { get; }

        /// <summary>
        ///     A function to start the server.
        /// </summary>
        /// <returns>The server task</returns>
        public Task StartAsync(CancellationToken token);

        /// <summary>
        ///     Stops the server.
        /// </summary>
        public Task StopAsync();
    }
}