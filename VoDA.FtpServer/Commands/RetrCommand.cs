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
        public async override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            if (string.IsNullOrWhiteSpace(args))
                return FoulderNotFound();
            args = NormalizationPath(args);
            if (fileSystem.ExistFile(client, NormalizationPath(Path.Join(client.Root, args))))
                args = NormalizationPath(Path.Join(client.Root, args));
            else if (fileSystem.ExistFile(client, NormalizationPath(args)))
                args = NormalizationPath(args);
            else
                return FoulderNotFound();
            client.SetupDataConnectionOperation(new DataConnectionOperation(client.RetrieveOperation, args));
            return CustomResponse(150, $"Opening {client.ConnectionType} mode data transfer for RETR");
        }
    }
}
