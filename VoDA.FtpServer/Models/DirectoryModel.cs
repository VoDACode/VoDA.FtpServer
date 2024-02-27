using System;

namespace VoDA.FtpServer.Models
{
    public class DirectoryModel
    {
        public DirectoryModel(string name, DateTime lastWriteTime)
        {
            Name = name;
            LastWriteTime = lastWriteTime;
        }

        public string Name { get; }
        public DateTime LastWriteTime { get; }
    }
}