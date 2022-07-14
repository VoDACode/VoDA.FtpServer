using System.IO;

using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    public delegate FileStream FileSystemUploadDelegate(IFtpClient client, string path);
}
