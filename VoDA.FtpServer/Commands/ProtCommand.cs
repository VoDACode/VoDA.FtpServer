using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("PROT")]
    internal class ProtCommand : BaseCommand
    {
        public override async Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions, string? args)
        {
            return CustomResponse(431, "Private");
        }
    }
}
