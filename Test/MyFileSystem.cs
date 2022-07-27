using System.Collections.Generic;
using System.IO;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace Test
{
    internal class MyFileSystem : VoDA.FtpServer.Contexts.FileSystemOptionsContext
    {
        private string rootPath { get; }
        public MyFileSystem()
        {
            rootPath = Path.GetTempPath();
            rootPath = rootPath.Remove(rootPath.Length - 1, 1);
        }

        public override Stream Append(IFtpClient client, string path)
        {
            return File.Open(Path.Join(rootPath, path), FileMode.Append);
        }

        public override bool Create(IFtpClient client, string path)
        {
            if (Directory.Exists(Path.Join(rootPath, path)))
                return false;
            Directory.CreateDirectory(Path.Join(rootPath, path));
            return true;
        }

        public override bool DeleteFile(IFtpClient client, string path)
        {
            if (!File.Exists(Path.Join(rootPath, path)))
                return false;
            File.Delete(Path.Join(rootPath, path));
            return true;
        }

        public override Stream Download(IFtpClient client, string path)
        {
            return new FileStream(Path.Join(rootPath, path), FileMode.Open);
        }

        public override bool ExistFile(IFtpClient client, string path)
        {
            return File.Exists(Path.Join(rootPath, path));
        }

        public override bool ExistFoulder(IFtpClient client, string path)
        {
            return Directory.Exists(Path.Join(rootPath, path));
        }

        public override long GetFileSize(IFtpClient client, string path)
        {
            var fileInfo = new FileInfo(Path.Join(rootPath, path));
            return fileInfo.Length;
        }

        public override (IReadOnlyList<DirectoryModel>, IReadOnlyList<FileModel>) List(IFtpClient client, string path)
        {
            List<FileModel> files = new List<FileModel>();
            List<DirectoryModel> directories = new List<DirectoryModel>();
            var f = Directory.GetFiles(Path.Join(rootPath, path));
            foreach (var file in f)
            {
                var info = new FileInfo(file);
                files.Add(new VoDA.FtpServer.Models.FileModel(info.Name, info.LastWriteTime, info.Length));
            }
            var d = Directory.GetDirectories(Path.Join(rootPath, path));
            foreach (var dir in d)
            {
                var info = new DirectoryInfo(dir);
                directories.Add(new VoDA.FtpServer.Models.DirectoryModel(info.Name, info.LastWriteTime));
            }

            return (directories, files);
        }

        public override bool DeleteFolder(IFtpClient client, string path)
        {
            if (!Directory.Exists(Path.Join(rootPath, path)))
                return false;
            Directory.Delete(Path.Join(rootPath, path), true);
            return true;
        }

        public override bool Rename(IFtpClient client, string from, string to)
        {
            var pathFrom = rootPath + Path.GetDirectoryName(from);
            var pathTo = rootPath + Path.GetDirectoryName(to);

            if (!Directory.Exists(pathTo) || !Directory.Exists(pathFrom))
                return false;
            File.Move(rootPath + from, rootPath + to);
            return true;
        }

        public override Stream Upload(IFtpClient client, string path)
        {
            return File.Create(Path.Join(rootPath, path));
        }
    }
}
