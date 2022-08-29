using System.Collections.Generic;
using System.IO;

using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace VoDA.FtpServer.Contexts
{
    public abstract class FileSystemOptionsContext
    {
        /// <summary>
        /// The function of renaming files or folders.
        /// </summary>
        /// <param name="client">An instance of the client making the request.</param>
        /// <param name="from">The old full path.</param>
        /// <param name="to">New full path.</param>
        /// <returns>true if the request was processed successfully, false - if elese</returns>
        public abstract bool Rename(IFtpClient client, string from, string to);

        /// <summary>
        /// File deletion function.
        /// </summary>
        /// <param name="client">An instance of the client making the request.</param>
        /// <param name="path">The full path to the file.</param>
        /// <returns>true if the request was processed successfully, false - if elese</returns>
        public abstract bool DeleteFile(IFtpClient client, string path);

        /// <summary>
        /// Folder deletion function.
        /// </summary>
        /// <param name="client">An instance of the client making the request.</param>
        /// <param name="path"></param>
        /// <returns>true if the request was processed successfully, false - if elese</returns>
        public abstract bool DeleteFolder(IFtpClient client, string path);

        /// <summary>
        /// Folder creation function.
        /// </summary>
        /// <param name="client">An instance of the client making the request.</param>
        /// <param name="path">The full path to the folder.</param>
        /// <returns>true if the request was processed successfully, false - if elese</returns>
        public abstract bool Create(IFtpClient client, string path);

        /// <summary>
        /// The function of checking the existence of a file.
        /// </summary>
        /// <param name="client">An instance of the client making the request.</param>
        /// <param name="path">The full path to the file.</param>
        /// <returns>true if the request was processed successfully, false - if elese</returns>
        public abstract bool ExistFile(IFtpClient client, string path);

        /// <summary>
        /// The function of checking the existence of a folder.
        /// </summary>
        /// <param name="client">An instance of the client making the request.</param>
        /// <param name="path">The full path to the folder.</param>
        /// <returns>true if the request was processed successfully, false - if elese</returns>
        public abstract bool ExistFoulder(IFtpClient client, string path);

        /// <summary>
        /// A function that processes a request to download a file.
        /// </summary>
        /// <param name="client">An instance of the client making the request.</param>
        /// <param name="path">The full path to the file.</param>
        /// <returns>The file <see cref="Stream"/>.</returns>
        public abstract Stream Download(IFtpClient client, string path);

        /// <summary>
        /// A function that processes a request to upload a file.
        /// </summary>
        /// <param name="client">An instance of the client making the request.</param>
        /// <param name="path">The full path to the file.</param>
        /// <returns>The file <see cref="Stream"/>.</returns>
        public abstract Stream Upload(IFtpClient client, string path);

        /// <summary>
        /// A function that handles a request to add data to a file.
        /// </summary>
        /// <param name="client">An instance of the client making the request.</param>
        /// <param name="path">The full path to the file.</param>
        /// <returns>The file <see cref="Stream"/>.</returns>
        public abstract Stream Append(IFtpClient client, string path);

        /// <summary>
        /// Handles a request to retrieve content in a folder.
        /// </summary>
        /// <param name="client">An instance of the client making the request.</param>
        /// <param name="path">The full path to the folder.</param>
        /// <returns>List of files and folders.</returns>
        public abstract (IReadOnlyList<DirectoryModel>, IReadOnlyList<FileModel>) List(IFtpClient client, string path);

        /// <summary>
        /// Handles a file size request.
        /// </summary>
        /// <param name="client">An instance of the client making the request.</param>
        /// <param name="path">The full path to the file.</param>
        /// <returns>The file size</returns>
        public abstract long GetFileSize(IFtpClient client, string path);
    }
}
