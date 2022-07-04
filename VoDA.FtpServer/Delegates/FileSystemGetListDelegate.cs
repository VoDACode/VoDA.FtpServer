using System.Collections.Generic;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Delegates
{
    public delegate (IReadOnlyList<DirectoryModel>, IReadOnlyList<FileModel>) FileSystemGetListDelegate(IFtpClient client, string path);
}
