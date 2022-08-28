using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Contexts;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("REIN")]
    internal class ReinCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            client.Username = string.Empty;
            client.PassiveListener = null;
            client.DataClient = null;
            client.IsAuthorized = false;
            return Task.FromResult(CustomResponse(220, "Service ready for new user"));
        }
    }
}
