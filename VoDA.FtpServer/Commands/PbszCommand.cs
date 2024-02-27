using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("PBSZ")]
    internal class PbszCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            return Task.FromResult(
                int.TryParse(args, out _) ? CustomResponse(200, "PBSZ=0") : CustomResponse(500, ""));
        }
    }
}