using System;
using System.Collections.Generic;
using System.IO;

using VoDA.FtpServer;
using VoDA.FtpServer.Interfaces;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new FtpServerBuilder()
                .ListenerSettings((config) =>
                {
                    config.Port = 5021;
                    config.ServerIp = System.Net.IPAddress.Any;
                })
                .Log((config) =>
                {
                    config.Level = LogLevel.Information;
                })
                .Certificate((config) =>
                {
                    config.CertificatePath = ".\\server.crt";
                    config.CertificateKey = ".\\server.key";
                })
                .Authorization<MyAuthorization>()
                .FileSystem<MyFileSystem>()
                .Build();
            server.Sessions.OnNewConnection += Sessions_OnNewConnection;
            server.Sessions.OnCloseConnection += Sessions_OnCloseConnection;
            server.StartAsync(System.Threading.CancellationToken.None).Wait();
        }

        private static void Sessions_OnCloseConnection(IFtpClient client, int id)
        {
            Console.WriteLine($"Close connect [{id}][{client.RemoteEndpoint}]'{client.Username}'");
        }

        private static void Sessions_OnNewConnection(IFtpClient client, int id)
        {
            Console.WriteLine($"New connect [{id}][{client.RemoteEndpoint}]'{client.Username}'");
        }
    }
}
