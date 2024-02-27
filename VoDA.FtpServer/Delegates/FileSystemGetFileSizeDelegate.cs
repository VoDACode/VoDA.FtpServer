using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    /// <param name="client">An instance of the client making the request.</param>
    /// <param name="path">The full path to the file.</param>
    /// <returns>The file size</returns>
    public delegate long FileSystemGetFileSizeDelegate(IFtpClient client, string path);
}