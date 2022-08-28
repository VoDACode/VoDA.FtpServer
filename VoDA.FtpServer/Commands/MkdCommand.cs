using System.IO;
using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Contexts;
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
            if (!configParameters.FileSystemOptions.Create(client, args))
                return Task.FromResult(CustomResponse(550, "Directory already exists"));
            return Task.FromResult(CustomResponse(250, "Requested file action okay, completed"));
        }
    }
}
