using Serilog;
using System.IO;
using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("CWD")]
    internal class CwdCommand : BaseCommand
    {
        public async override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorization authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            if (string.IsNullOrWhiteSpace(args))
                return FoulderNotFound();
            if(fileSystem.ExistFoulder(client, NormalizationPath(Path.Join(client.Root, args))))
                args = NormalizationPath(Path.Join(client.Root, args));
            else if(fileSystem.ExistFoulder(client, NormalizationPath(args)))
                args = NormalizationPath(args);
            else
                return FoulderNotFound();
            client.Root = args;
            return ChangedToNewDirectory();
        }
    }
}
