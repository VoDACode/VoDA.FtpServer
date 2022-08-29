using System.IO;

using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    /// <param name="client">An instance of the client making the request.</param>
    /// <param name="path">The full path to the file.</param>
    /// <returns>The file <see cref="Stream"/>.</returns>
    public delegate Stream FileSystemAppendDelegate(IFtpClient client, string path);
}
