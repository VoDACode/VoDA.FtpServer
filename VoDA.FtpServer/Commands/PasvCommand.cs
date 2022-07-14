using System;
using System.Net;
using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("PASV")]
    internal class PasvCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions, string? args)
        {
            client.ConnectionType = Enums.ConnectionType.Passive;
            if (client.TcpSocket?.Client.LocalEndPoint == null)
                return Task.FromResult(UnknownCommandParameter());
            IPAddress localAddress = ((IPEndPoint)client.TcpSocket.Client.LocalEndPoint).Address;
            client.PassiveListener = new System.Net.Sockets.TcpListener(localAddress, 0);
            client.PassiveListener.Start();
            IPEndPoint localEndpoint = (IPEndPoint)client.PassiveListener.LocalEndpoint;
            byte[] address = localEndpoint.Address.GetAddressBytes();
            short port = (short)localEndpoint.Port;
            byte[] portArray = BitConverter.GetBytes(port);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(portArray);
            return Task.FromResult(CustomResponse(227, $"Entering Passive Mode ({address[0]},{address[1]},{address[2]},{address[3]},{portArray[0]},{portArray[1]})"));
        }
    }
}
