using System;
using System.Collections;
using System.Collections.Generic;

namespace VoDA.FtpServer.Interfaces
{
    public interface ISessionsController : IEnumerable
    {
        public event Action<IFtpClient, int>? OnNewConnection;
        public event Action<IFtpClient, int>? OnCloseConnection;
        public event Action<IFtpClient, int, long, long>? OnUploadProgress;
        public event Action<IFtpClient, int, long, long>? OnDownloadProgress;
        public IReadOnlyDictionary<int, IFtpClient> Sessions { get; }
        public IFtpClient this[int id] { get; }
        public int Count { get; }
        public bool Kik(int id);
    }
}
