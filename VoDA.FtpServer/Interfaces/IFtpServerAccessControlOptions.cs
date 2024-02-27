using System.Collections.Generic;
using System.Net;

namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpServerAccessControlOptions
    {
        /// <summary>
        ///     Enables or disables the connection filter.
        /// </summary>
        public bool EnableConnectionFiltering { get; set; }

        /// <summary>
        ///     Changes the filter mode.
        ///     <list type="table">true - Ban addresses are listed</list>
        ///     <list type="table">false - Allow addresses are listed</list>
        /// </summary>
        public bool BlacklistMode { get; set; }

        /// <summary>
        ///     List of addresses
        /// </summary>
        public List<IPAddress> Filters { get; }
    }
}