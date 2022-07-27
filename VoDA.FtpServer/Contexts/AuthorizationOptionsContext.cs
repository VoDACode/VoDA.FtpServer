namespace VoDA.FtpServer.Contexts
{
    public abstract class AuthorizationOptionsContext
    {
        public abstract bool UseAuthorization { get; set; }

        public abstract bool TryUsernameVerification(string username);
        public abstract bool TryPasswordVerification(string username, string password);
    }
}
