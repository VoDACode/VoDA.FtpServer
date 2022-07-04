﻿using VoDA.FtpServer.Delegates;

namespace VoDA.FtpServer.Interfaces
{
    public interface IFtpServerFileSystemOptions
    {
        public event FileSystemRenameDelegate OnRename;
        public event FileSystemDeleteDelegate OnDelete;
        public event FileSystemDeleteDelegate OnRemoveDir;
        public event FileSystemCreateDelegate OnCreate;
        public event FileSystemExistDelegate OnExistFile;
        public event FileSystemExistDelegate OnExistFoulder;
        public event FileSystemDownloadDelegate OnDownload;
        public event FileSystemUploadDelegate OnUpload;
        public event FileSystemAppendDelegate OnAppend;
        public event FileSystemGetListDelegate OnGetList;
    }
}
