using System.IO;
using System.Linq;
using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Contexts;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("CDUP")]
    internal class CdupCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, AuthorizationOptionsContext authorization, FileSystemOptionsContext fileSystem, FtpServerOptions serverOptions,string? args)
        {
            string[] tmpPath = client.Root.Split(Path.DirectorySeparatorChar);
            client.Root = string.Join(Path.DirectorySeparatorChar, tmpPath.Take(tmpPath.Length - 1));
            return Task.FromResult(ChangedToNewDirectory());
        }
    }
}
