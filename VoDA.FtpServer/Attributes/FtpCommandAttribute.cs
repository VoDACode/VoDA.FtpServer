using System;

namespace VoDA.FtpServer.Attributes
{
    public class FtpCommandAttribute : Attribute
    {
        public string Command { get; }
        public FtpCommandAttribute(string command)
        {
            Command = command.ToUpper();
        }
    }
}
