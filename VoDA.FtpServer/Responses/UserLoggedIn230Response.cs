namespace VoDA.FtpServer.Responses
{
    internal class UserLoggedIn230Response : BaseResponse
    {
        public override string Text => "User logged in";
        public override int Code => 230;
    }
}
