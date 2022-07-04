using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    public delegate bool FileSystemCreateDelegate(IFtpClient client, string path);
}
