using VoDA.FtpServer.Delegates;

namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpServerFileSystemOptions
    {
        /// <summary>
        /// It is called when a request is made to rename files or folders.
        /// </summary>
        public event FileSystemRenameDelegate OnRename;

        /// <summary>
        /// File deletion function.
        /// </summary>
        public event FileSystemDeleteDelegate OnDeleteFile;

        /// <summary>
        /// Folder deletion function.
        /// </summary>
        public event FileSystemDeleteDelegate OnDeleteFolder;

        /// <summary>
        /// Called when a request to create a folder is made.
        /// </summary>
        public event FileSystemCreateDelegate OnCreate;

        /// <summary>
        /// Called to check for the existence of a file.
        /// </summary>
        public event FileSystemExistDelegate OnExistFile;

        /// <summary>
        /// Called to check for the existence of a folder.
        /// </summary>
        public event FileSystemExistDelegate OnExistFoulder;

        /// <summary>
        /// Called to process a file download request.
        /// </summary>
        public event FileSystemDownloadDelegate OnDownload;

        /// <summary>
        /// Called to process a file upload request.
        /// </summary>
        public event FileSystemUploadDelegate OnUpload;

        /// <summary>
        /// Called to process a file append request.
        /// </summary>
        public event FileSystemAppendDelegate OnAppend;

        /// <summary>
        /// Handles a request to retrieve content in a folder.
        /// </summary>
        public event FileSystemGetListDelegate OnGetList;

        /// <summary>
        /// Handles a file size request.
        /// </summary>
        public event FileSystemGetFileSizeDelegate OnGetFileSize;

        /// <summary>
        /// Handles a request to retrieve the modified date of a file.
        /// </summary>
        public event FileSystemFileModificationTimeDelegate OnGetFileModificationTime;
    }
}
