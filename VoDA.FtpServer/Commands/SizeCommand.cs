using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Contexts;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("SIZE")]
    internal class SizeCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            if (args == null || !configParameters.FileSystemOptions.ExistFile(client, NormalizationPath(args)))
                return Task.FromResult(FileNotFound());
            return Task.FromResult(CustomResponse(213, configParameters.FileSystemOptions.GetFileSize(client, NormalizationPath(args)).ToString()));
        }
    }
}
