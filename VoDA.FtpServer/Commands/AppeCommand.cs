using System.IO;
using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("APPE")]
    internal class AppeCommand : BaseCommand
    {
        public async override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            args = NormalizationPath(args);
            args = Path.Join(client.Root, args);
            args = NormalizationPath(args);
            if (args == null)
                return CustomResponse(450, "Requested file action not taken");
            client.SetupDataConnectionOperation(new DataConnectionOperation(client.AppendOperation, args));
            return CustomResponse(150, $"Opening {client.ConnectionType} mode data transfer for APPE");
        }
    }
}
