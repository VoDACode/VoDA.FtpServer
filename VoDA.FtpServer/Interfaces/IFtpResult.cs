namespace VoDA.FtpServer.Interfaces
{
    internal interface IFtpResult
    {
        public string Text { get; }
        public int Code { get; }
    }
}
