using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    /// <param name="client">An instance of the client making the request.</param>
    /// <param name="from">The old full path.</param>
    /// <param name="to">New full path.</param>
    /// <returns>true if the request was processed successfully, false - if elese</returns>
    public delegate bool FileSystemRenameDelegate(IFtpClient client, string from, string to);
}