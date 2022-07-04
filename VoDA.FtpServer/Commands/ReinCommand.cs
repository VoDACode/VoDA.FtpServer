using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("REIN")]
    internal class ReinCommand : BaseCommand
    {
        public async override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorization authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            client.Username = null;
            client.PassiveListener = null;
            client.TcpSocket = null;
            return CustomResponse(220, "Service ready for new user");
        }
    }
}
