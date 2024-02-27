using System;

namespace VoDA.FtpServer.Attributes
{
    public class FtpCommandAttribute : Attribute
    {
        public FtpCommandAttribute(string command)
        {
            Command = command.ToUpper();
        }

        public string Command { get; }
    }
}