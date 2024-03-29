﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Enums;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("PASV")]
    internal class PasvCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            client.ConnectionType = ConnectionType.Passive;
            if (client.TcpSocket?.Client.LocalEndPoint == null)
                return Task.FromResult(UnknownCommandParameter());
            var localAddress = ((IPEndPoint)client.TcpSocket.Client.LocalEndPoint).Address;
            client.PassiveListener = new TcpListener(localAddress, 0);
            client.PassiveListener.Start();
            var localEndpoint = (IPEndPoint)client.PassiveListener.LocalEndpoint;
            var address = localEndpoint.Address.GetAddressBytes();
            var port = (short)localEndpoint.Port;
            var portArray = BitConverter.GetBytes(port);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(portArray);
            return Task.FromResult(CustomResponse(227,
                $"Entering Passive Mode ({address[0]},{address[1]},{address[2]},{address[3]},{portArray[0]},{portArray[1]})"));
        }
    }
}