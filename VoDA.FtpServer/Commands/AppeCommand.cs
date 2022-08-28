using System.IO;
using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Contexts;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("APPE")]
    internal class AppeCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            args = NormalizationPath(args);
            args = Path.Join(client.Root, args);
            args = NormalizationPath(args);
            if (args == null)
                return Task.FromResult(CustomResponse(450, "Requested file action not taken"));
            client.SetupDataConnectionOperation(new DataConnectionOperation(client.AppendOperation, args));
            return Task.FromResult(CustomResponse(150, $"Opening {client.ConnectionType} mode data transfer for APPE"));
        }
    }
}
