using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [Authorize]
    [FtpCommand("PASS")]
    internal class PassCommand : BaseCommand
    {
        public async override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorization authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            if(!authorization.TryPasswordVerification(client.Username, args))
                return NotLoggedIn();
            client.IsAuthorized = true;
            return UserLoggedIn();
        }
    }
}
