using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;
using VoDA.FtpServer.Responses;

namespace VoDA.FtpServer.Commands
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public abstract class BaseCommand
    {
        public abstract Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string? args);

        protected string NormalizationPath(string? path)
        {
            if (path == null)
                return string.Empty;

            path = Path.DirectorySeparatorChar switch
            {
                '/' when path.Contains('\\') => path.Replace('\\', '/'),
                '\\' when path.Contains('/') => path.Replace('/', '\\'),
                _ => path
            };
            int length;
            var dsc = Path.DirectorySeparatorChar.ToString();
            do
            {
                length = path.Length;
                path = path.Replace($"{dsc}{dsc}", dsc);
                path = path.Replace($"{dsc}..", dsc);
            } while (length != path.Length);

            return path;
        }

        protected IFtpResult UnknownCommand()
        {
            return new UnknownCommand502Response();
        }

        protected IFtpResult ServiceReady()
        {
            return new ServiceReady220Response();
        }

        protected IFtpResult CloseConnection()
        {
            return new CloseConnection221Response();
        }

        protected IFtpResult UsernameOk()
        {
            return new UsernameOk331Response();
        }

        protected IFtpResult UserLoggedIn()
        {
            return new UserLoggedIn230Response();
        }

        protected IFtpResult NotLoggedIn()
        {
            return new NotLoggedIn530Response();
        }

        protected IFtpResult ChangedToNewDirectory()
        {
            return new ChangeWorkingDirectory250Response();
        }

        protected IFtpResult UnknownCommandParameter()
        {
            return new UnknownCommandParameter504Response();
        }

        protected IFtpResult Error()
        {
            return new Error500Response();
        }

        protected IFtpResult Ok()
        {
            return new Ok200Response();
        }

        protected IFtpResult FileNotFound()
        {
            return CustomResponse(550, "File Not Found");
        }

        protected IFtpResult FolderNotFound()
        {
            return CustomResponse(550, "Folder Not Found");
        }

        protected IFtpResult CustomResponse(int code, string data)
        {
            return new CustomResponse(data, code);
        }
    }
}