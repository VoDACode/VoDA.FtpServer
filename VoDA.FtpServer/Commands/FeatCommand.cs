using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Contexts;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("FEAT")]
    internal class FeatCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, AuthorizationOptionsContext authorization, FileSystemOptionsContext fileSystem, FtpServerOptions serverOptions, string? args)
        {
            client.StreamWriter?.WriteLine("221 - Extensions supported:");
            client.StreamWriter?.WriteLine(" SIZE");
            return Task.FromResult(CustomResponse(221, "End"));
        }
    }
}
