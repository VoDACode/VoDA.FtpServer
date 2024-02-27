using System.IO;
using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("RETR")]
    internal class RetrCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            if (string.IsNullOrWhiteSpace(args))
                return Task.FromResult(FolderNotFound());
            args = NormalizationPath(args);
            if (configParameters.FileSystemOptions.ExistFile(client, NormalizationPath(Path.Join(client.Root, args))))
                args = NormalizationPath(Path.Join(client.Root, args));
            else if (configParameters.FileSystemOptions.ExistFile(client, NormalizationPath(args)))
                args = NormalizationPath(args);
            else
                return Task.FromResult(FolderNotFound());
            client.SetupDataConnectionOperation(new DataConnectionOperation(client.RetrieveOperation, args));
            return Task.FromResult(CustomResponse(150, $"Opening {client.ConnectionType} mode data transfer for RETR"));
        }
    }
}