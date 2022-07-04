using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [Authorize]
    [FtpCommand("AUTH")]
    internal class AuthCommand : BaseCommand
    {
        public override async Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorization authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            if (string.IsNullOrWhiteSpace(serverOptions.Certificate.CertificatePath))
                return UnknownCommandParameter();
            if (args == "TLS")
                return CustomResponse(234, "Enabling TLS Connection");
            return CustomResponse(504, "Unrecognized AUTH mode");
        }
    }
}
