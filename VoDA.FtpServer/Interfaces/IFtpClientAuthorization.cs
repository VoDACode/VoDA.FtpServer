namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpClientAuthorization
    {
        public bool IsAuthorized { get; }
        public string Username { get; set; }
        public string Password { get; set; }
        public void Fail();
        public void Succeed();
    }
}
