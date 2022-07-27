namespace Test
{
    internal class MyAuthorization : VoDA.FtpServer.Contexts.AuthorizationOptionsContext
    {
        public override bool UseAuthorization { get; set; } = true;

        public override bool TryPasswordVerification(string username, string password)
        {
            return username == "VoDA" && password == "123";
        }

        public override bool TryUsernameVerification(string username)
        {
            return username == "VoDA";
        }
    }
}
