using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoDA.FtpServer.Attributes
{
    internal class FtpCommandAttribute : Attribute
    {
        public string Command { get; }
        public FtpCommandAttribute(string command)
        {
            Command = command.ToUpper();
        }
    }
}
