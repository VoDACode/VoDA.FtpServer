using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Contexts;
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
            string[] splitArgs = args.Split(' ');
            IFtpResult result = Error();
            switch (splitArgs[0])
            {
                case "A":
                    client.TransferType = Enums.TransferType.Ascii;
                    result = CustomResponse(200, "Type set to A");
                    break;
                case "I":
                    client.TransferType = Enums.TransferType.Image;
                    result = CustomResponse(200, "Type set to I");
                    break;
                default:
                    result = UnknownCommandParameter();
                    break;
            }
            if(splitArgs.Length > 1)
                switch(splitArgs[1])
                {
                    case "N":
                        result = Ok();
                        break;
                    case "T":
                    case "C":
                    default:
                        result = UnknownCommandParameter();
                        break;
                }
            return Task.FromResult(result);
        }
    }
}
