namespace VoDA.FtpServer.Responses
{
    internal class Error500Response : BaseResponse
    {
        public override string Text => "ERROR";

        public override int Code => 500;
    }
}