using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("ABOR")]
    internal class AborCommand : BaseCommand
    {
        public override async Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions, string? args)
        {
            client.StopLastCommand();
            return CustomResponse(226, "");
        }
    }
}
