using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Contexts;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("MODE")]
    internal class ModeCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            if (args != null && args.ToUpper() == "S")
                return Task.FromResult(Ok());
            return Task.FromResult(UnknownCommandParameter());
        }
    }
}
