using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VoDA.FtpServer.Contexts
{
    public abstract class AccessControlOptionsContext
    {
        public abstract bool EnableСonnectionСiltering { get; set; }
        public abstract bool BlacklistMode { get; set; }
        public abstract List<IPAddress> Filters { get; }
    }
}
