namespace VoDA.FtpServer.Responses
{
    internal class ServiceReady220Response : BaseResponse
    {
        public override string Text => "Service Ready";

        public override int Code => 220;
    }
}
