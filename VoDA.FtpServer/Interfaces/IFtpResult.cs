namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpResult
    {
        public string Text { get; }
        public int Code { get; }
        public string ToString();
    }
}