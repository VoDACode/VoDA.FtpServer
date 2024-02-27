using System;
using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("STOU")]
    internal class StouCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            var path = new Guid().ToString();
            path = NormalizationPath(path);
            client.SetupDataConnectionOperation(new DataConnectionOperation(client.StoreOperation, path));
            return Task.FromResult(CustomResponse(150, $"Opening {client.ConnectionType} mode data transfer for STOU"));
        }
    }
}