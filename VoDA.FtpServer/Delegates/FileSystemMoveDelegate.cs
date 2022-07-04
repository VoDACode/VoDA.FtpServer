using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    public delegate bool FileSystemMoveDelegate(IFtpClient client, string from, string to);
}
