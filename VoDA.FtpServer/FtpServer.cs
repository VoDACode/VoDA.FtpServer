using Serilog;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using VoDA.FtpServer.Controllers;
using VoDA.FtpServer.Contexts;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;
using System.Net;
using System.Linq;

namespace VoDA.FtpServer
{
    internal class FtpServer : IFtpServerControl
    {
        private TcpListener _serverSocket;
        FtpServerParameters _serverParameters;
        private bool _isEnable = false;
        private Task? _handlerTask;
        private CancellationToken cancellation;
        private SessionsController sessionsController = new SessionsController();
        public ISessionsController Sessions => sessionsController;

        public FtpServer(FtpServerParameters parameters)
        {
            _serverParameters = parameters;
            _serverSocket = new TcpListener(_serverParameters.serverOptions.ServerIp, _serverParameters.serverOptions.Port);
            var logger = new LoggerConfiguration();
            if (_serverParameters.serverLogOptions.Level != LogLevel.None)
                logger.WriteTo.Console();
            if (_serverParameters.serverLogOptions.Level == LogLevel.Information)
                logger.MinimumLevel.Information();
            else if (_serverParameters.serverLogOptions.Level == LogLevel.Debug)
                logger.MinimumLevel.Debug();
            Log.Logger = logger.CreateLogger();
        }

        public Task StartAsync(CancellationToken token)
        {
            if (_isEnable)
                throw new Exception("Server is runing.");
            _isEnable = true;
            cancellation = token;
            _serverSocket.Start();
            Log.Information($"Server is running (ftp://localhost:{_serverParameters.serverOptions.Port}/)");
            if (_serverParameters.serverOptions.MaxConnections > 0)
            {
                Log.Information($"Enabled limited connections mode.");
                Log.Information($"Max count connections = {_serverParameters.serverOptions.MaxConnections}");
            }
            if (_serverParameters.serverOptions.ServerIp != System.Net.IPAddress.Any)
            {
                Log.Information($"Enabled filter connections mode.");
                Log.Information($"Waiting connection from {_serverParameters.serverOptions.ServerIp}");
            }
            return handler(token);
        }

        public Task StopAsync()
        {
            if (!_isEnable)
                throw new Exception("Server isn`t runing.");
            _isEnable = false;
            return Task.CompletedTask;
        }

        private Task handler(CancellationToken token)
        {
            _handlerTask = Task.Run(() =>
            {
                while (!token.IsCancellationRequested && _isEnable)
                {
                    var tcp = _serverSocket.AcceptTcpClient();
                    var sw = new StreamWriter(tcp.GetStream());
                    if (tcp.Client.RemoteEndPoint == null)
                    {
                        closeConnection(tcp, sw);
                        continue;
                    }
                    IPEndPoint? RemoteEndpoint = tcp.Client.RemoteEndPoint as IPEndPoint;
                    if (RemoteEndpoint == null)
                    {
                        closeConnection(tcp, sw);
                        continue;
                    }
                    if (_serverParameters.serverAccessControl.EnableСonnectionСiltering)
                    {
                        if (_serverParameters.serverAccessControl.BlacklistMode == _serverParameters.serverAccessControl.Filters.Any(p => p.Equals(RemoteEndpoint.Address)))
                        {
                            closeConnection(tcp, sw, "221 Access is denied.");
                            continue;
                        }
                    }
                    if (_serverParameters.serverOptions.MaxConnections > 0
                    && sessionsController.Count >= _serverParameters.serverOptions.MaxConnections)
                    {
                        closeConnection(tcp, sw, "221 The server is full!");
                        continue;
                    }
                    var client = new FtpClient(tcp);
                    sessionsController.Add(client);
                    client.HandleClient(_serverParameters);
                }
                _serverSocket.Stop();
            });
            return _handlerTask;
        }

        private void closeConnection(TcpClient tcp, StreamWriter sw, string? message = null)
        {
            if (message != null)
            {
                sw.WriteLine(message);
                sw.Flush();
                sw.Close();
            }
            tcp.Close();
        }
    }
}
