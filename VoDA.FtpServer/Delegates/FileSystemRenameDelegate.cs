using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    public delegate bool FileSystemRenameDelegate(IFtpClient client, string from, string to);
}
