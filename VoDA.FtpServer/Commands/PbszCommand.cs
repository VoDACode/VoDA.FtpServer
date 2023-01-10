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
            int size = 0;
            if(!int.TryParse(args, out size))
                return Task.FromResult(CustomResponse(500, ""));
            return Task.FromResult(CustomResponse(200, "PBSZ=0"));
        }
    }
}
