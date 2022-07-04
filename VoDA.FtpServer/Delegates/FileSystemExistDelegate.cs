using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    public delegate bool FileSystemExistDelegate(IFtpClient client, string path);
}
