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
        public override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            var path = args ?? client.Root;
            path = NormalizationPath(path);
            path = Path.Join(client.Root, args);
            path = NormalizationPath(path);
            if (path == null)
                return Task.FromResult(CustomResponse(450, "Requested file action not taken"));
            if (path.Length >= 2 && path.Substring(path.Length - 2) == "-a")
            {
                path = path.Substring(0, path.Length - 2);
            }
            if(!fileSystem.ExistFoulder(client, path))
                return Task.FromResult(CustomResponse(450, "Requested file action not taken"));
            client.SetupDataConnectionOperation(new DataConnectionOperation(client.ListOperation, path));
            return Task.FromResult(CustomResponse(150, $"Opening {client.ConnectionType} mode data transfer for LIST"));
        }
    }
}
