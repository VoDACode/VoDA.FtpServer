using VoDA.FtpServer.Delegates;

namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpServerAuthorizationOptions
    {
        /// <summary>
        /// Enables or disables the use of authorization.
        /// </summary>
        public bool UseAuthorization { get; set; }
        /// <summary>
        /// Called to verify the correctness of the username.
        /// </summary>
        public event AuthorizationUsernameDelegate UsernameVerification;
        /// <summary>
        /// Called to verify the correctness of the user's password.
        /// </summary>
        public event AuthorizationDelegate PasswordVerification;
    }
}
