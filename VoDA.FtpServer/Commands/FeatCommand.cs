using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("FEAT")]
    internal class FeatCommand : BaseCommand
    {
        public override async Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorization authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions, string? args)
        {
            client.StreamWriter.WriteLine("221 - Extensions supported:");
            client.StreamWriter.WriteLine(" SIZE");
            return CustomResponse(221, "End");
        }
    }
}
