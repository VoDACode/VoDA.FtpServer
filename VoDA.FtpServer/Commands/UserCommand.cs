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
        public override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            if (args == null || !authorization.TryUsernameVerification(args))
                return Task.FromResult(NotLoggedIn());
            client.Username = args;
            return Task.FromResult(UsernameOk());
        }
    }
}
