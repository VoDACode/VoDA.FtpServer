using System.Collections.Generic;
using System.IO;

using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Contexts
{
    public abstract class FileSystemOptionsContext
    {
        public abstract bool Rename(IFtpClient client, string from, string to);
        public abstract bool DeleteFile(IFtpClient client, string path);
        public abstract bool DeleteFolder(IFtpClient client, string path);
        public abstract bool Create(IFtpClient client, string path);
        public abstract bool ExistFile(IFtpClient client, string path);
        public abstract bool ExistFoulder(IFtpClient client, string path);
        public abstract Stream Download(IFtpClient client, string path);
        public abstract Stream Upload(IFtpClient client, string path);
        public abstract Stream Append(IFtpClient client, string path);
        public abstract (IReadOnlyList<DirectoryModel>, IReadOnlyList<FileModel>) List(IFtpClient client, string path);
        public abstract long GetFileSize(IFtpClient client, string path);
    }
}
