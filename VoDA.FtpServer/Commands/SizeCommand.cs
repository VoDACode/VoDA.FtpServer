using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("SIZE")]
    internal class SizeCommand : BaseCommand
    {
        public override async Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorization authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions, string? args)
        {
            if (args == null || !fileSystem.ExistFile(client, NormalizationPath(args)))
                return FileNotFound();
            return CustomResponse(213, fileSystem.GetFileSize(client, NormalizationPath(args)).ToString());
        }
    }
}
