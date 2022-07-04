namespace VoDA.FtpServer.Responses
{
    internal class UsernameOk331Response : BaseResponse
    {
        public override string Text => "Username ok, need password";

        public override int Code => 331;
    }
}
