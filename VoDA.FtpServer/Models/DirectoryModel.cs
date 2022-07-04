using System;

namespace VoDA.FtpServer.Models
{
    public class DirectoryModel
    {
        public string Name { get; }
        public DateTime LastWriteTime { get; }
        public DirectoryModel(string name, DateTime lastWriteTime)
        {
            Name = name;
            LastWriteTime = lastWriteTime;
        }
    }
}
