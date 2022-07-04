namespace VoDA.FtpServer.Responses
{
    internal class Ok200Response : BaseResponse
    {
        public override string Text => "OK";

        public override int Code => 200;
    }
}
