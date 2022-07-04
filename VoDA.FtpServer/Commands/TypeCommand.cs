using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Commands
{
    [FtpCommand("TYPE")]
    internal class TypeCommand : BaseCommand
    {
        public async override Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorization authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args)
        {
            string[] splitArgs = args.Split(' ');
            IFtpResult result = Error();
            switch (splitArgs[0])
            {
                case "A":
                    client.TransferType = Enums.TransferType.Ascii;
                    result = Ok();
                    break;
                case "I":
                    client.TransferType = Enums.TransferType.Image;
                    result = Ok();
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
            return result;
        }
    }
}
