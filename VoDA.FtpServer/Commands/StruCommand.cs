using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Enums;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("STRU")]
    internal class StruCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            switch (args)
            {
                case "F":
                    client.FileStructureType = FileStructureType.File;
                    break;
                case "R":
                case "P":
                    return Task.FromResult(CustomResponse(504, $"STRU not implemented for \"{args}\""));
                default:
                    return Task.FromResult(CustomResponse(501, $"Parameter {args} not recognized"));
            }
            return Task.FromResult(Ok());
        }
    }
}
