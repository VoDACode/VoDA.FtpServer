using System.Collections.Generic;
using System.IO;
using VoDA.FtpServer.Delegates;
using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Models
{
    internal class FtpServerFileSystemOptions : IFtpServerFileSystemOptions
    {
        public event FileSystemRenameDelegate OnRename;
        public event FileSystemDeleteDelegate OnDelete;
        public event FileSystemDeleteDelegate OnRemoveDir;
        public event FileSystemCreateDelegate OnCreate;
        public event FileSystemExistDelegate OnExistFile;
        public event FileSystemDownloadDelegate OnDownload;
        public event FileSystemUploadDelegate OnUpload;
        public event FileSystemAppendDelegate OnAppend;
        public event FileSystemGetListDelegate OnGetList;
        public event FileSystemExistDelegate OnExistFoulder;
        public event FileSystemGetFileSizeDelegate OnGetFileSize;

        public bool Rename(IFtpClient client, string from, string to)
            => OnRename == null ? false : OnRename.Invoke(client, from, to);
        public bool Delete(IFtpClient client, string path)
            => OnDelete == null ? false : OnDelete.Invoke(client, path);
        public bool RemoveDir(IFtpClient client, string path)
            => OnRemoveDir == null ? false : OnRemoveDir.Invoke(client, path);
        public bool Create(IFtpClient client, string path)
            => OnCreate == null ? false : OnCreate.Invoke(client, path);
        public bool ExistFile(IFtpClient client, string path)
            => OnExistFile == null ? false : OnExistFile.Invoke(client, path);
        public bool ExistFoulder(IFtpClient client, string path)
        => OnExistFoulder == null ? false : OnExistFoulder.Invoke(client, path);
        public FileStream? Download(IFtpClient client, string path)
            => OnDownload == null ? null : OnDownload.Invoke(client, path);
        public FileStream? Upload(IFtpClient client, string path)
            => OnUpload == null ? null : OnUpload.Invoke(client, path);
        public FileStream? Append(IFtpClient client, string path)
            => OnAppend == null ? null : OnAppend.Invoke(client, path);
        public (IReadOnlyList<DirectoryModel>, IReadOnlyList<FileModel>) List(IFtpClient client, string path)
        {
            if (OnGetList is null)
                return (new List<DirectoryModel>(), new List<FileModel>());
            return OnGetList.Invoke(client, path);
        }
        public long GetFileSize(IFtpClient client, string path)
            => OnGetFileSize == null ? -1 : OnGetFileSize.Invoke(client, path);
            
    }
}
