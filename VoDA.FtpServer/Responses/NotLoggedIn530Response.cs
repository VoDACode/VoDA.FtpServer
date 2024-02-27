namespace VoDA.FtpServer.Responses
{
    internal class NotLoggedIn530Response : BaseResponse
    {
        public override string Text => "Not logged in";
        public override int Code => 530;
    }
}