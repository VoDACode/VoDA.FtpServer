using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Contexts;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [Authorize]
    [FtpCommand("PASS")]
    internal class PassCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, AuthorizationOptionsContext authorization, FileSystemOptionsContext fileSystem, FtpServerOptions serverOptions,string? args)
        {
            if(args == null || !authorization.TryPasswordVerification(client.Username, args))
                return Task.FromResult(NotLoggedIn());
            client.IsAuthorized = true;
            return Task.FromResult(UserLoggedIn());
        }
    }
}
