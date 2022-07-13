using VoDA.FtpServer.Delegates;
using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Models
{
    internal class FtpServerAuthorizationOptions : IFtpServerAuthorizationOptions, IValidConfig
    {
        public bool UseAuthorization { get; set; } = false;

        public event AuthorizationUsernameDelegate UsernameVerification;
        public event AuthorizationDelegate PasswordVerification;

        public bool TryUsernameVerification(string username)
        {
            if (!UseAuthorization)
                return true;
            bool? status = UsernameVerification?.Invoke(username);
            if(status == true)
                return true;
            return false;
        }
        public bool TryPasswordVerification(string username, string password)
        {
            if (!UseAuthorization)
                return true;
            bool? status = PasswordVerification?.Invoke(username, password);
            if (status == true)
                return true;
            return false;
        }

        public void Valid()
        {
            if (UseAuthorization)
            {
                if(UsernameVerification == null)
                    throw new System.NotImplementedException("UsernameVerification");
                if (PasswordVerification == null)
                    throw new System.NotImplementedException("PasswordVerification");
            }
        }
    }
}
