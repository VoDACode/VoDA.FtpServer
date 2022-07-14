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
        public override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            if (args == null || args.Length == 0)
                return Task.FromResult(UnknownCommandParameter());
            client.ConnectionType = ConnectionType.Active;

            char delimiter = args[0];

            string[] rawSplit = args.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

            string ipAddress = rawSplit[1];
            string port = rawSplit[2];

            client.DataEndpoint = new IPEndPoint(IPAddress.Parse(ipAddress), int.Parse(port));
            return Task.FromResult(CustomResponse(200, "Data connection is established"));
        }
    }
}
