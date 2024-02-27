using System.Collections.Generic;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Delegates
{
    /// <param name="client">An instance of the client making the request.</param>
    /// <param name="path">The full path to the folder.</param>
    /// <returns>List of files and folders.</returns>
    public delegate (IReadOnlyList<DirectoryModel>, IReadOnlyList<FileModel>) FileSystemGetListDelegate(
        IFtpClient client, string path);
}