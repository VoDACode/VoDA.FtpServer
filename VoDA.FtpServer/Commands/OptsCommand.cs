using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("opts")]
    internal class OptsCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions, string? args)
        {
            if (args == null)
                return Task.FromResult(Error());
            return Task.FromResult(CustomResponse(202, "UTF8 mode is always enabled. No need to send this command"));
        }
    }
}
