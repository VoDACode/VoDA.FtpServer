using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("MDTM")]
    internal class MdtmCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            args = NormalizationPath(args);
            if (!configParameters.FileSystemOptions.ExistFile(client, args))
                return Task.FromResult(FileNotFound());
            return Task.FromResult(CustomResponse(213, configParameters.FileSystemOptions.GetFileModificationTime(client, args).ToString("yyyyMMddHHmmss.fff")));
        }
    }
}
