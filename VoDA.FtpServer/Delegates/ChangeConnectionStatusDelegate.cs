using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Delegates
{
    /// <param name="client">An instance of the client making the request.</param>
    /// <param name="id">Session Id.</param>
    public delegate void ChangeConnectionStatusDelegate(IFtpClient client, int id);
}
