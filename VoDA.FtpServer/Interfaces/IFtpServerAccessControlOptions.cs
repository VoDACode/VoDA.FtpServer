using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpServerAccessControlOptions
    {
        public bool EnableСonnectionСiltering { get; set; }
        public List<IPAddress> Filters { get; }
        public bool BlacklistMode { get; set; }
    }
}
