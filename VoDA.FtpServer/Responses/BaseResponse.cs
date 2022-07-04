using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Responses
{
    internal abstract class BaseResponse : IFtpResult
    {
        public abstract string Text { get; }
        public abstract int Code { get; }
        public override string ToString()
        {
            return $"{Code} {Text}";
        }
    }
}
