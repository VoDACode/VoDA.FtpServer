using System.Collections;
using System.Collections.Generic;
using VoDA.FtpServer.Delegates;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Controllers
{
    internal class SessionsController : ISessionsController
    {
        private readonly Dictionary<int, IFtpClient> _sessions = new();
        public IReadOnlyDictionary<int, IFtpClient> Sessions => _sessions;

        public IFtpClient this[int id] => _sessions[id];

        public int Count => _sessions.Count;

        public event ChangeConnectionStatusDelegate? OnNewConnection;
        public event ChangeConnectionStatusDelegate? OnCloseConnection;
        public event ClientFileProcessingDelegate? OnUploadProgress;
        public event ClientFileProcessingDelegate? OnDownloadProgress;

        public event ClientDataProcessingStatusDelegate? OnStartUpload;
        public event ClientDataProcessingStatusDelegate? OnCompleteUpload;
        public event ClientDataProcessingStatusDelegate? OnStartDownload;
        public event ClientDataProcessingStatusDelegate? OnCompleteDownload;

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

        public int Add(FtpClient value)
        {
            var id = _sessions.Count;
            _sessions.Add(id, value);
            value.OnConnection += item => OnNewConnection?.Invoke(item, id);
            value.OnEndProcessing += item => OnCloseConnection?.Invoke(item, id);
            value.OnUploadProgress += (item, len, done) => OnUploadProgress?.Invoke(item, id, len, done);
            value.OnDownloadProgress += (item, len, done) => OnDownloadProgress?.Invoke(item, id, len, done);
            value.OnStartUpload += (item, file) => OnStartUpload?.Invoke(item, file);
            value.OnCompleteUpload += (item, file) => OnCompleteUpload?.Invoke(item, file);
            value.OnStartDownload += (item, file) => OnStartDownload?.Invoke(item, file);
            value.OnCompleteDownload += (item, file) => OnCompleteDownload?.Invoke(item, file);
            return id;
        }
    }
}