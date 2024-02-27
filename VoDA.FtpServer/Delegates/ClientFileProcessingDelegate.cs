using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    /// <param name="client">An instance of the client making the request.</param>
    /// <param name="id">Session Id.</param>
    /// <param name="fileSize">Full file size.</param>
    /// <param name="processedSize">Processed file size.</param>
    public delegate void ClientFileProcessingDelegate(IFtpClient client, int id, long fileSize, long processedSize);
}