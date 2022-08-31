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
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            client.StreamWriter?.WriteLine("211 - Extensions supported:");
            client.StreamWriter?.WriteLine(" SIZE");
            client.StreamWriter?.WriteLine(" MDTM");
            return Task.FromResult(CustomResponse(211, "END"));
        }
    }
}
