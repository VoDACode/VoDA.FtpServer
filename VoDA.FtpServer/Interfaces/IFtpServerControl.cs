using System.Threading.Tasks;
using System.Threading;

namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpServerControl
    {
        public Task StartAsync(CancellationToken token);
        public Task StopAsync();
    }
}
