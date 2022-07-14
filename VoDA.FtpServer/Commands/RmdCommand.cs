using System.IO;
using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("RMD")]
    internal class RmdCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            args = NormalizationPath(args);
            args = Path.Join(client.Root, args);
            args = NormalizationPath(args);
            if (!fileSystem.RemoveDir(client, args))
                return Task.FromResult(CustomResponse(550, "Directory Not Found"));
            return Task.FromResult(CustomResponse(250, "Requested file action okay, completed"));
        }
    }
}
