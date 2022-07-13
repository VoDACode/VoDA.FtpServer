using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("CDUP")]
    internal class CdupCommand : BaseCommand
    {
        public override async Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            string[] tmpPath = client.Root.Split(Path.DirectorySeparatorChar);
            client.Root = string.Join(Path.DirectorySeparatorChar, tmpPath.Take(tmpPath.Length - 1));
            return ChangedToNewDirectory();
        }
    }
}
