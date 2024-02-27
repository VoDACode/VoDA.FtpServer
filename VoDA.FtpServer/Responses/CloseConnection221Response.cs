namespace VoDA.FtpServer.Responses
{
    internal class CloseConnection221Response : BaseResponse
    {
        public override string Text => "Service closing control connection";

        public override int Code => 221;
    }
}