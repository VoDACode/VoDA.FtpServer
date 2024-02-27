using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    /// <param name="client">An instance of the client making the request.</param>
    /// <param name="file">File path.</param>
    public delegate void ClientDataProcessingStatusDelegate(IFtpClient client, string file);
}