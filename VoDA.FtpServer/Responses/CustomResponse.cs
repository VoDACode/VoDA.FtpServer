namespace VoDA.FtpServer.Responses
{
    internal class CustomResponse : BaseResponse
    {
        public override string Text { get; }

        public override int Code { get; }
        public CustomResponse(string text, int code)
        {
            Text = text;
            Code = code;
        }
    }
}
