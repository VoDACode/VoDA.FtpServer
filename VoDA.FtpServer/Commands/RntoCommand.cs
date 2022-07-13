using System.IO;
using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("RNTO")]
    internal class RntoCommand : BaseCommand
    {
        public async override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            args = NormalizationPath(args);
            args = Path.Join(client.Root, args);
            args = NormalizationPath(args);
            if (string.IsNullOrWhiteSpace(client.RenameFrom) || string.IsNullOrWhiteSpace(args))
                return CustomResponse(450, "Requested file action not taken");
            if (!fileSystem.Rename(client, client.RenameFrom, args))
                return CustomResponse(403, "Access denied");
            return CustomResponse(250, "Requested file action okay, completed");
        }
    }
}
