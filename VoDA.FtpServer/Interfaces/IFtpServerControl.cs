using System.Threading.Tasks;
using System.Threading;

namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpServerControl
    {
        /// <summary>
        /// A function to start the server.
        /// </summary>
        /// <returns>The server task</returns>
        public Task StartAsync(CancellationToken token);

        /// <summary>
        /// Stops the server.
        /// </summary>
        public Task StopAsync();

        /// <summary>
        /// Client session management interface.
        /// </summary>
        public ISessionsController Sessions { get; }
    }
}
