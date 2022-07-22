using System.IO;

using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    public delegate Stream FileSystemDownloadDelegate(IFtpClient client, string path);
}
