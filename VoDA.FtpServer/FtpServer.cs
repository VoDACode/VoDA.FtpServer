using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using VoDA.FtpServer.Controllers;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer
{
    internal class FtpServer : IFtpServerControl
    {
        private readonly FtpServerParameters _serverParameters;
        private readonly TcpListener _serverSocket;
        private readonly SessionsController _sessionsController = new();
        private Task? _handlerTask;
        private bool _isEnable;

        public FtpServer(FtpServerParameters parameters)
        {
            _serverParameters = parameters;
            _serverSocket = new TcpListener(_serverParameters.serverOptions.ServerIp,
                _serverParameters.serverOptions.Port);
            var logger = new LoggerConfiguration();
            if (_serverParameters.serverLogOptions.Level != LogLevel.None)
                logger.WriteTo.Console();
            if (_serverParameters.serverLogOptions.Level == LogLevel.Information)
                logger.MinimumLevel.Information();
            else if (_serverParameters.serverLogOptions.Level == LogLevel.Debug)
                logger.MinimumLevel.Debug();
            Log.Logger = logger.CreateLogger();
        }

        public ISessionsController Sessions => _sessionsController;

        public Task StartAsync(CancellationToken token)
        {
            if (_isEnable)
                throw new Exception("Server is running.");
            _isEnable = true;
            _serverSocket.Start();
            Log.Information($"Server is running (ftp://localhost:{_serverParameters.serverOptions.Port}/)");
            if (_serverParameters.serverOptions.MaxConnections > 0)
            {
                Log.Information("Enabled limited connections mode.");
                Log.Information($"Max count connections = {_serverParameters.serverOptions.MaxConnections}");
            }

            if (!IPAddress.Any.Equals(_serverParameters.serverOptions.ServerIp))
            {
                Log.Information("Enabled filter connections mode.");
                Log.Information($"Waiting connection from {_serverParameters.serverOptions.ServerIp}");
            }

            return Handler(token);
        }

        public Task StopAsync()
        {
            if (!_isEnable)
                throw new Exception("Server isn`t running.");
            _isEnable = false;
            return Task.CompletedTask;
        }

        private Task Handler(CancellationToken token)
        {
            _handlerTask = Task.Run(() =>
            {
                while (!token.IsCancellationRequested && _isEnable)
                {
                    var tcp = _serverSocket.AcceptTcpClient();
                    var sw = new StreamWriter(tcp.GetStream());
                    if (tcp.Client.RemoteEndPoint == null)
                    {
                        CloseConnection(tcp, sw);
                        continue;
                    }

                    if (tcp.Client.RemoteEndPoint is not IPEndPoint remoteEndpoint)
                    {
                        CloseConnection(tcp, sw);
                        continue;
                    }

                    if (_serverParameters.serverAccessControl.EnableConnectionFiltering)
                        if (_serverParameters.serverAccessControl.BlacklistMode ==
                            _serverParameters.serverAccessControl.Filters.Any(p => p.Equals(remoteEndpoint.Address)))
                        {
                            CloseConnection(tcp, sw, "221 Access is denied.");
                            continue;
                        }

                    if (_serverParameters.serverOptions.MaxConnections > 0
                        && _sessionsController.Count >= _serverParameters.serverOptions.MaxConnections)
                    {
                        CloseConnection(tcp, sw, "221 The server is full!");
                        continue;
                    }

                    var client = new FtpClient(tcp);
                    _sessionsController.Add(client);
                    client.HandleClient(_serverParameters);
                }

                _serverSocket.Stop();
            }, token);
            return _handlerTask;
        }

        private void CloseConnection(TcpClient tcp, StreamWriter sw, string? message = null)
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