using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;
using VoDA.FtpServer.Responses;

namespace VoDA.FtpServer.Commands
{
    internal abstract class BaseCommand
    {
        public abstract Task<IFtpResult> Invoke(FtpClient client, FtpServerAuthorization authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions,string? args);

        protected string NormalizationPath(string path)
        {
            if(path == null)
                return string.Empty;

            if (Path.DirectorySeparatorChar == '/' && path.Contains('\\'))
                path = path.Replace('\\', '/');
            else if (Path.DirectorySeparatorChar == '\\' && path.Contains('/'))
                path = path.Replace('/', '\\');
            var len = 0;
            string dsc = Path.DirectorySeparatorChar.ToString();
            do
            {
                len = path.Length;
                path = path.Replace($"{dsc}{dsc}", dsc);
                path = path.Replace($"{dsc}..", dsc);
            } while (len != path.Length);
            return path;
        }

        protected IFtpResult UnknownCommand() => new UnknownCommand502Response();
        protected IFtpResult ServiceReady() => new ServiceReady220Response();
        protected IFtpResult CloseConnection() => new CloseConnection221Response();
        protected IFtpResult UsernameOk() => new UsernameOk331Response();
        protected IFtpResult UserLoggedIn() => new UserLoggedIn230Response();
        protected IFtpResult NotLoggedIn() => new NotLoggedIn530Response();
        protected IFtpResult ChangedToNewDirectory() => new ChangeWorkingDirectory250Response();
        protected IFtpResult UnknownCommandParameter() => new UnknownCommandParameter504Response();
        protected IFtpResult Error() => new Error500Response();
        protected IFtpResult Ok() => new Ok200Response();

        protected IFtpResult FileNotFound()
            => CustomResponse(550, "File Not Found");
        protected IFtpResult FoulderNotFound()
            => CustomResponse(550, "Foulder Not Found");

        protected IFtpResult CustomResponse(int code, string data)
            => new CustomResponse(data, code);
    }
}
