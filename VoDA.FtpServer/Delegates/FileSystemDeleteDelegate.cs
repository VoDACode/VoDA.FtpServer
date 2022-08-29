using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    /// <param name="client">An instance of the client making the request.</param>
    /// <param name="path">The full path to the file.</param>
    /// <returns>true if the request was processed successfully, false - if elese</returns>
    public delegate bool FileSystemDeleteDelegate(IFtpClient client, string path);
}
