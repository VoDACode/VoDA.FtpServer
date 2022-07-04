using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    public delegate bool FileSystemDeleteDelegate(IFtpClient client, string path);
}
