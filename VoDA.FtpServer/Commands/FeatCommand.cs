using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("FEAT")]
    internal class FeatCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            // ReSharper disable StringLiteralTypo
            client.StreamWriter?.WriteLine("211-Extensions supported:");
            client.StreamWriter?.WriteLine(" SIZE");
            client.StreamWriter?.WriteLine(" MDTM");
            client.StreamWriter?.WriteLine(" AUTH TLS");
            client.StreamWriter?.WriteLine(" PROT");
            client.StreamWriter?.WriteLine(" UTF8");
            client.StreamWriter?.WriteLine(" EPRT");
            client.StreamWriter?.WriteLine(" PBSZ");
            foreach (var command in FtpCommandHandler.Instance.Commands)
                if (command.Value.IsCustom)
                    client.StreamWriter?.WriteLine($" {command.Key}");
            return Task.FromResult(CustomResponse(211, "END"));
            // ReSharper restore StringLiteralTypo
        }
    }
}