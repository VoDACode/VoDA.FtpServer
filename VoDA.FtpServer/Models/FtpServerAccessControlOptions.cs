using System;
using System.Collections.Generic;
using System.Net;

using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Models
{
    internal class FtpServerAccessControlOptions : Contexts.AccessControlOptionsContext, IFtpServerAccessControlOptions
    {
        public override bool EnableConnectionFiltering { get; set; } = false;

        public override List<IPAddress> Filters { get; } = new List<IPAddress>();

        public override bool BlacklistMode { get; set; } = true;
    }
}
