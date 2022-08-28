using System;
using System.Net;
using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Enums;
using VoDA.FtpServer.Contexts;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("PORT")]
    internal class PortCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            if (args == null)
                return Task.FromResult(UnknownCommandParameter());
            client.ConnectionType = ConnectionType.Active;

            string[] ipAndPort = args.Split(',');

            byte[] ipAddress = new byte[4];
            byte[] port = new byte[2];

            for (int i = 0; i < 4; i++)
            {
                ipAddress[i] = Convert.ToByte(ipAndPort[i]);
            }

            for (int i = 4; i < 6; i++)
            {
                port[i - 4] = Convert.ToByte(ipAndPort[i]);
            }

            if (BitConverter.IsLittleEndian)
                Array.Reverse(port);
            try
            {
                client.DataEndpoint = new IPEndPoint(new IPAddress(ipAddress), BitConverter.ToInt16(port, 0));
            }
            catch (Exception ex)
            {
                throw new Exception($"PORT: '{port[0]}','{port[0]}'\nARGS: '{args}'\n\n----------------------\n\n{ex}");
            }
            return Task.FromResult(CustomResponse(200, "Data connection is established"));
        }
    }
}
