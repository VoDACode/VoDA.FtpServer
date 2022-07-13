using VoDA.FtpServer.Delegates;

namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpServerAuthorizationOptions
    {
        public bool UseAuthorization { get; set; }
        public event AuthorizationUsernameDelegate UsernameVerification;
        public event AuthorizationDelegate PasswordVerification;
    }
}
