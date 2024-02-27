#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using VoDA.FtpServer.Contexts;
using VoDA.FtpServer.Delegates;
using VoDA.FtpServer.Interfaces;

namespace VoDA.FtpServer.Models
{
    internal class FtpServerFileSystemOptions : FileSystemOptionsContext, IFtpServerFileSystemOptions, IValidConfig
    {
        public event FileSystemRenameDelegate OnRename;
        public event FileSystemDeleteDelegate OnDeleteFile;
        public event FileSystemDeleteDelegate OnDeleteFolder;
        public event FileSystemCreateDelegate OnCreate;
        public event FileSystemExistDelegate OnExistFile;
        public event FileSystemDownloadDelegate OnDownload;
        public event FileSystemUploadDelegate OnUpload;
        public event FileSystemAppendDelegate OnAppend;
        public event FileSystemGetListDelegate OnGetList;
        public event FileSystemExistDelegate OnExistFoulder;
        public event FileSystemGetFileSizeDelegate OnGetFileSize;
        public event FileSystemFileModificationTimeDelegate OnGetFileModificationTime;

        public void Valid()
        {
            if (OnAppend == null)
                throw new ArgumentNullException(nameof(OnAppend));
            if (OnCreate == null)
                throw new ArgumentNullException(nameof(OnCreate));
            if (OnDeleteFile == null)
                throw new ArgumentNullException(nameof(OnDeleteFile));
            if (OnDownload == null)
                throw new ArgumentNullException(nameof(OnDownload));
            if (OnExistFile == null)
                throw new ArgumentNullException(nameof(OnExistFile));
            if (OnExistFoulder == null)
                throw new ArgumentNullException(nameof(OnExistFoulder));
            if (OnGetFileSize == null)
                throw new ArgumentNullException(nameof(OnGetFileSize));
            if (OnGetList == null)
                throw new ArgumentNullException(nameof(OnGetList));
            if (OnDeleteFolder == null)
                throw new ArgumentNullException(nameof(OnDeleteFolder));
            if (OnRename == null)
                throw new ArgumentNullException(nameof(OnRename));
            if (OnUpload == null)
                throw new ArgumentNullException(nameof(OnUpload));
        }

        public override bool Rename(IFtpClient client, string from, string to)
        {
            return OnRename == null ? false : OnRename.Invoke(client, from, to);
        }

        public override bool DeleteFile(IFtpClient client, string path)
        {
            return OnDeleteFile == null ? false : OnDeleteFile.Invoke(client, path);
        }

        public override bool DeleteFolder(IFtpClient client, string path)
        {
            return OnDeleteFolder == null ? false : OnDeleteFolder.Invoke(client, path);
        }

        public override bool Create(IFtpClient client, string path)
        {
            return OnCreate == null ? false : OnCreate.Invoke(client, path);
        }

        public override bool ExistFile(IFtpClient client, string path)
        {
            return OnExistFile == null ? false : OnExistFile.Invoke(client, path);
        }

        public override bool ExistFolder(IFtpClient client, string path)
        {
            return OnExistFoulder == null ? false : OnExistFoulder.Invoke(client, path);
        }

        public override Stream Download(IFtpClient client, string path)
        {
            return OnDownload.Invoke(client, path);
        }

        public override Stream Upload(IFtpClient client, string path)
        {
            return OnUpload.Invoke(client, path);
        }

        public override Stream Append(IFtpClient client, string path)
        {
            return OnAppend.Invoke(client, path);
        }

        public override (IReadOnlyList<DirectoryModel>, IReadOnlyList<FileModel>) List(IFtpClient client, string path)
        {
            if (OnGetList is null)
                return (new List<DirectoryModel>(), new List<FileModel>());
            return OnGetList.Invoke(client, path);
        }

        public override long GetFileSize(IFtpClient client, string path)
        {
            return OnGetFileSize == null ? -1 : OnGetFileSize.Invoke(client, path);
        }

        public override DateTime GetFileModificationTime(IFtpClient client, string path)
        {
            return OnGetFileModificationTime.Invoke(client, path);
        }
    }
}