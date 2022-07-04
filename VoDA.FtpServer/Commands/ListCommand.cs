using System.IO;
using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("LIST")]
    internal class ListCommand : BaseCommand
    {
        public async override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorization authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            var path = args ?? client.Root;
            path = NormalizationPath(path);
            path = Path.Join(client.Root, args);
            path = NormalizationPath(path);
            if (path == null)
                return CustomResponse(450, "Requested file action not taken");
            client.SetupDataConnectionOperation(new DataConnectionOperation(client.ListOperation, path));
            return CustomResponse(150, $"Opening {client.ConnectionType} mode data transfer for LIST");
        }
    }
}
