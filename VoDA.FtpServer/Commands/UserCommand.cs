using System.Threading.Tasks;

using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Contexts;

namespace VoDA.FtpServer.Commands
{
    [Authorize]
    [FtpCommand("USER")]
    internal class UserCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            if (args == null || !configParameters.AuthorizationOptions.TryUsernameVerification(args))
                return Task.FromResult(NotLoggedIn());
            client.Username = args;
            return Task.FromResult(UsernameOk());
        }
    }
}
