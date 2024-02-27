using System;
using VoDA.FtpServer.Contexts;
using VoDA.FtpServer.Delegates;
using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Models
{
    internal class FtpServerAuthorizationOptions : AuthorizationOptionsContext, IFtpServerAuthorizationOptions,
        IValidConfig
    {
        public override bool UseAuthorization { get; set; } = false;

        public event AuthorizationUsernameDelegate? UsernameVerification;
        public event AuthorizationDelegate? PasswordVerification;

        public void Valid()
        {
            if (UseAuthorization)
            {
                if (UsernameVerification == null)
                    throw new NotImplementedException("UsernameVerification");
                if (PasswordVerification == null)
                    throw new NotImplementedException("PasswordVerification");
            }
        }

        public override bool TryUsernameVerification(string username)
        {
            if (!UseAuthorization)
                return true;
            var status = UsernameVerification?.Invoke(username);
            if (status == true)
                return true;
            return false;
        }

        public override bool TryPasswordVerification(string username, string password)
        {
            if (!UseAuthorization)
                return true;
            var status = PasswordVerification?.Invoke(username, password);
            if (status == true)
                return true;
            return false;
        }
    }
}