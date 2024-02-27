using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("opts")]
    internal class OptsCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            return Task.FromResult(args == null
                ? Error()
                : CustomResponse(202, "UTF8 mode is always enabled. No need to send this command"));
        }
    }
}