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
            if (string.IsNullOrWhiteSpace(args) || !fileSystem.ExistFoulder(client, Path.Join(client.Root, args)))
                return FoulderNotFound();
            client.Root = Path.Join(client.Root, args);
            return ChangedToNewDirectory();
        }
    }
}
