using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Enums;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("TYPE")]
    internal class TypeCommand : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args)
        {
            if (args == null)
                return Task.FromResult(UnknownCommandParameter());
            var splitArgs = args.Split(' ');
            IFtpResult result;
            switch (splitArgs[0])
            {
                case "A":
                    client.TransferType = TransferType.Ascii;
                    result = CustomResponse(200, "Type set to A");
                    break;
                case "I":
                    client.TransferType = TransferType.Image;
                    result = CustomResponse(200, "Type set to I");
                    break;
                default:
                    result = UnknownCommandParameter();
                    break;
            }

            if (splitArgs.Length <= 1) return Task.FromResult(result);

            result = splitArgs[1] switch
            {
                "N" => Ok(),
                _ => UnknownCommandParameter()
            };
            return Task.FromResult(result);
        }
    }
}