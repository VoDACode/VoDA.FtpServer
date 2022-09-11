namespace VoDA.FtpServer.Responses
{
    internal class UsernameOk331Response : BaseResponse
    {
        public override string Text => "Please, specify the password.";

        public override int Code => 331;
    }
}
