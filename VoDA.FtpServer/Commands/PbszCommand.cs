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
    [FtpCommand("PBSZ")]
    internal class PbszCommand : BaseCommand
    {
        public override async Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions, string? args)
        {
            int size = 0;
            if(!int.TryParse(args, out size) || size < 32)
                return CustomResponse(200, "4096");
            client.BufferSize = size;
            return Ok();
        }
    }
}
