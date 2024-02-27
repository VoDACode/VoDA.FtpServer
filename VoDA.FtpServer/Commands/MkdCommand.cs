using System.IO;
using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("MKD")]
    internal class MkdCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            args = NormalizationPath(args);
            args = Path.Join(client.Root, args);
            args = NormalizationPath(args);

            var result = configParameters.FileSystemOptions.Create(client, args)
                ? CustomResponse(250, "Requested file action okay, completed")
                : CustomResponse(550, "Directory already exists");

            return Task.FromResult(result);
        }
    }
}