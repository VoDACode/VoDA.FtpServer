using System;
using System.Net;
using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Enums;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("EPRT")]
    internal class EprtCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            if (string.IsNullOrEmpty(args))
                return Task.FromResult(UnknownCommandParameter());
            client.ConnectionType = ConnectionType.Active;

            var delimiter = args[0];

            var rawSplit = args.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

            var ipAddress = rawSplit[1];
            var port = rawSplit[2];

            client.DataEndpoint = new IPEndPoint(IPAddress.Parse(ipAddress), int.Parse(port));
            return Task.FromResult(CustomResponse(200, "Data connection is established"));
        }
    }
}