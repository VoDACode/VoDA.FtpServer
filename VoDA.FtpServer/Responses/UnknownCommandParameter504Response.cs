namespace VoDA.FtpServer.Responses
{
    internal class UnknownCommandParameter504Response : BaseResponse
    {
        public override string Text => "Command not implemented for that parameter";
        public override int Code => 504;
    }
}