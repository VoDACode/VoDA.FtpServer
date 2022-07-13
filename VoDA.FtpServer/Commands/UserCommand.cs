using System.Threading.Tasks;

using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;
using VoDA.FtpServer.Attributes;

namespace VoDA.FtpServer.Commands
{
    [Authorize]
    [FtpCommand("USER")]
    internal class UserCommand : BaseCommand
    {
        public override async Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            if (!authorization.TryUsernameVerification(args))
                return NotLoggedIn();
            client.Username = args;
            return UsernameOk();
        }
    }
}
