using System.Collections.Generic;
using System.IO;
using VoDA.FtpServer;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rootPath = Path.GetTempPath();
            rootPath = rootPath.Remove(rootPath.Length - 1, 1);

            var server = new FtpServerBuilder()
                .ListenerSettings((config) =>
                {
                    config.Port = 5021;
                    config.ServerIp = System.Net.IPAddress.Any;
                })
                .Certificate((config) =>
                {

                    config.CertificatePath = ".\\server.crt";
                    config.CertificateKey = ".\\server.key";
                })
                .Authorization((config) =>
                {
                    config.UseAuthorization = true;
                    config.UsernameVerification += Auth_UsernameVerification;
                    config.PasswordVerification += Auth_PasswordVerification;
                })
                .FileSystem((fs) =>
                {
                    fs.OnDelete += (client, path) =>
                    {
                        if (!File.Exists(Path.Join(rootPath, path)))
                            return false;
                        File.Delete(Path.Join(rootPath, path));
                        return true;
                    };
                    fs.OnRename += (client, from, to) =>
                    {
                        var pathFrom = rootPath + Path.GetDirectoryName(from);
                        var pathTo = rootPath + Path.GetDirectoryName(to);

                        if (!Directory.Exists(pathTo) || !Directory.Exists(pathFrom))
                            return false;
                        File.Move(rootPath + from, rootPath + to);
                        return true;
                    };
                    fs.OnDownload += (client, path) =>
                    {
                        return new FileStream(Path.Join(rootPath, path), FileMode.Open);
                    };
                    fs.OnGetList += (client, path) =>
                    {
                        List<VoDA.FtpServer.Models.FileModel> files = new List<VoDA.FtpServer.Models.FileModel>();
                        List<VoDA.FtpServer.Models.DirectoryModel> directories = new List<VoDA.FtpServer.Models.DirectoryModel>();
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
                    };
                    fs.OnExistFile += (client, path) =>
                    {
                        return File.Exists(Path.Join(rootPath, path));
                    };
                    fs.OnExistFoulder += (client, path) =>
                    {
                        return Directory.Exists(Path.Join(rootPath, path));
                    };
                    fs.OnCreate += (client, path) =>
                    {
                        if (Directory.Exists(Path.Join(rootPath, path)))
                            return false;
                        Directory.CreateDirectory(Path.Join(rootPath, path));
                        return true;
                    };
                    fs.OnAppend += (client, path) =>
                    {
                        return File.Open(Path.Join(rootPath, path), FileMode.Open);
                    };
                    fs.OnRemoveDir += (client, path) =>
                    {
                        if (!Directory.Exists(Path.Join(rootPath, path)))
                            return false;
                        Directory.Delete(Path.Join(rootPath, path), true);
                        return true;
                    };
                    fs.OnUpload += (client, path) =>
                    {
                        return File.Create(Path.Join(rootPath, path));
                    };
                    fs.OnGetFileSize += (client, path) =>
                    {
                        var fileInfo = new FileInfo(Path.Join(rootPath, path));
                        return fileInfo.Length;
                    };
                })
                .Build();
            server.StartAsync(System.Threading.CancellationToken.None).Wait();
        }

        private static bool Auth_PasswordVerification(string username, string password)
        {
            return username == "VoDA" && password == "123";
        }

        private static bool Auth_UsernameVerification(string username)
        {
            return username == "VoDA";
        }
    }
}
