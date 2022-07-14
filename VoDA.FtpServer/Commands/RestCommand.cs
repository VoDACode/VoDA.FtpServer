using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("REST")]
    internal class RestCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions, string? args)
        {
            if (!long.TryParse(args, out var len))
                return Task.FromResult(UnknownCommandParameter());
            client.RestoreLastCommand(len);
            return Task.FromResult(CustomResponse(350, ""));
        }
    }
}
