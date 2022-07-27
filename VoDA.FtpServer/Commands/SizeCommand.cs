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
        public override Task<IFtpResult> Invoke(FtpClient client, AuthorizationOptionsContext authorization, FileSystemOptionsContext fileSystem, FtpServerOptions serverOptions, string? args)
        {
            if (args == null || !fileSystem.ExistFile(client, NormalizationPath(args)))
                return Task.FromResult(FileNotFound());
            return Task.FromResult(CustomResponse(213, fileSystem.GetFileSize(client, NormalizationPath(args)).ToString()));
        }
    }
}
