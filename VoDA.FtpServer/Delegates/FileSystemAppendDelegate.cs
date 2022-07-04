using System.IO;
using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    public delegate FileStream FileSystemAppendDelegate(IFtpClient client, string path);
}
