using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Contexts;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [Authorize]
    [FtpCommand("AUTH")]
    internal class AuthCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, AuthorizationOptionsContext authorization, FileSystemOptionsContext fileSystem, FtpServerOptions serverOptions, string? args)
        {
            if (args == "TLS")
                return Task.FromResult(CustomResponse(234, "Enabling TLS Connection"));
            return Task.FromResult(CustomResponse(504, "Unrecognized AUTH mode"));
        }
    }
}
