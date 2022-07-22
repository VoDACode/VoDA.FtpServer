using System.IO;

using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    public delegate Stream FileSystemUploadDelegate(IFtpClient client, string path);
}
