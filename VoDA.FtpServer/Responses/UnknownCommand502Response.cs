namespace VoDA.FtpServer.Responses
{
    internal class UnknownCommand502Response : BaseResponse
    {
        public override string Text => "Unknown command";
        public override int Code => 502;
    }
}
