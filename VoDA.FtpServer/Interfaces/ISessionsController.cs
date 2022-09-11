using System;
using System.Collections;
using System.Collections.Generic;
using VoDA.FtpServer.Delegates;

namespace VoDA.FtpServer.Interfaces
{
    /// <summary>
    /// Client session management interface.
    /// </summary>
    public interface ISessionsController : IEnumerable
    {
        /// <summary>
        /// Called upon new connection.
        /// </summary>
        public event ChangeConnectionStatusDelegate? OnNewConnection;

        /// <summary>
        /// Called after the connection is closed.
        /// </summary>
        public event ChangeConnectionStatusDelegate? OnCloseConnection;

        /// <summary>
        /// Called every time the server receives part of the user's file.
        /// </summary>
        public event ClientFileProcessingDelegate? OnUploadProgress;

        /// <summary>
        /// Called every time the server sends a part of the file to the user.
        /// </summary>
        public event ClientFileProcessingDelegate? OnDownloadProgress;

        /// <summary>
        /// Called when the user begins to upload a file. 
        /// </summary>
        public event ClientDataProcessingStatusDelegate OnStartUpload;

        /// <summary>
        /// Called when the user finished uploading a file.
        /// </summary>
        public event ClientDataProcessingStatusDelegate OnCompleteUpload;

        /// <summary>
        /// Called when the user begins to download a file. 
        /// </summary>
        public event ClientDataProcessingStatusDelegate OnStartDownload;

        /// <summary>
        /// Called when the user finished downloading a file.
        /// </summary>
        public event ClientDataProcessingStatusDelegate OnCompleteDownload;

        /// <summary>
        /// List of active sessions.
        /// </summary>
        public IReadOnlyDictionary<int, IFtpClient> Sessions { get; }
        public IFtpClient this[int id] { get; }

        /// <summary>
        /// Number of active sessions.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Kill the selected session.
        /// </summary>
        /// <param name="id">Session Id.</param>
        /// <returns><see cref="true"/> if closed successfully, else <see cref="false"/>.</returns>
        public bool Kik(int id);
    }
}
