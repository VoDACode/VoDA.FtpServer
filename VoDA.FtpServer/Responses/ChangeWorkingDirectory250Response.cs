namespace VoDA.FtpServer.Responses
{
    internal class ChangeWorkingDirectory250Response : BaseResponse
    {
        public override string Text => "Changed to new directory";

        public override int Code => 250;
    }
}
