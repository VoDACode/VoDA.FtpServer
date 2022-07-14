using System.IO;

using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    public delegate FileStream FileSystemDownloadDelegate(IFtpClient client, string path);
}
