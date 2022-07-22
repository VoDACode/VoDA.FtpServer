using System.IO;

using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    public delegate Stream FileSystemAppendDelegate(IFtpClient client, string path);
}
