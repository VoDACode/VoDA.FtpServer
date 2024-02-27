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
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            args = NormalizationPath(args);
            args = Path.Join(client.Root, args);
            args = NormalizationPath(args);
            if (string.IsNullOrWhiteSpace(client.RenameFrom) || string.IsNullOrWhiteSpace(args))
                return Task.FromResult(CustomResponse(450, "Requested file action not taken"));
            if (!configParameters.FileSystemOptions.Rename(client, client.RenameFrom, args))
                return Task.FromResult(CustomResponse(403, "Access denied"));
            return Task.FromResult(CustomResponse(250, "Requested file action okay, completed"));
        }
    }
}