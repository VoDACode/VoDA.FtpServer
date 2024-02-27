using System.Collections.Generic;
using System.Net;

namespace VoDA.FtpServer.Contexts
{
    public abstract class AccessControlOptionsContext
    {
        /// <summary>
        /// Enables or disables the connection filter.
        /// </summary>
        public abstract bool EnableConnectionFiltering { get; set; }
        /// <summary>
        /// Changes the filter mode.
        /// <list type="table">true - Ban addresses are listed</list>
        /// <list type="table">false - Allow addresses are listed</list>
        /// </summary>
        public abstract bool BlacklistMode { get; set; }
        /// <summary>
        /// List of addresses
        /// </summary>
        public abstract List<IPAddress> Filters { get; }
    }
}
