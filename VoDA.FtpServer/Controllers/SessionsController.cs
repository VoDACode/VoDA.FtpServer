using System;
using System.Collections;
using System.Collections.Generic;

using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Controllers
{
    internal class SessionsController : ISessionsController
    {
        private Dictionary<int, FtpClient> _sessions = new Dictionary<int, FtpClient>();
        public IReadOnlyDictionary<int, IFtpClient> Sessions => (IReadOnlyDictionary<int, IFtpClient>)_sessions;

        public IFtpClient this[int id] => _sessions[id];

        public int Count => _sessions.Count;

        public event Action<IFtpClient, int>? OnNewConnection;
        public event Action<IFtpClient, int>? OnCloseConnection;
        public event Action<IFtpClient, int, long, long>? OnUploadProgress;
        public event Action<IFtpClient, int, long, long>? OnDownloadProgress;

        public int Add(FtpClient value)
        {
            var id = _sessions.Count;
            _sessions.Add(id, value);
            value.OnConnection += (item) => OnNewConnection?.Invoke(item, id);
            value.OnEndProcessing += (item) => OnCloseConnection?.Invoke(item, id);
            value.OnUploadProgress += (item, len, done) => OnUploadProgress?.Invoke(item, id, len, done);
            value.OnDownloadProgress += (item, len, done) => OnDownloadProgress?.Invoke(item, id, len, done);
            return id;
        }

        public IEnumerator GetEnumerator()
        {
            return _sessions.GetEnumerator();
        }

        public bool Kik(int id)
        {
            if (!_sessions.ContainsKey(id))
                return false;
            _sessions[id].Kik();
            _sessions.Remove(id);
            return true;
        }
    }
}
