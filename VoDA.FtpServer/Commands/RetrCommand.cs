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
        public async override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorization authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            args = NormalizationPath(args);
            args = Path.Join(client.Root, args);
            args = NormalizationPath(args);
            if (args == null || fileSystem.ExistFile(client, args))
                return FileNotFound();
            client.SetupDataConnectionOperation(new DataConnectionOperation(client.RetrieveOperation, args));
            return CustomResponse(150, $"Opening {client.ConnectionType} mode data transfer for RETR");
        }
    }
}
