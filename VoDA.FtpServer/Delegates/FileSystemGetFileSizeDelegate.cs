using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    public delegate long FileSystemGetFileSizeDelegate(IFtpClient client, string path);
}
