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
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            var path = args ?? client.Root;
            path = args != null && args.Contains('.') ? client.Root : path;
            path = NormalizationPath(path);
            path = Path.Join(client.Root, args);
            path = NormalizationPath(path);

            if (path.Length >= 2 && path[^2..] == "-a") path = path[..^2];
            if (!configParameters.FileSystemOptions.ExistFoulder(client, path))
                return Task.FromResult(CustomResponse(450, "Requested file action not taken"));
            client.SetupDataConnectionOperation(new DataConnectionOperation(client.ListOperation, path));
            return Task.FromResult(CustomResponse(150, $"Opening {client.ConnectionType} mode data transfer for LIST"));
        }
    }
}