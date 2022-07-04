using System;

namespace VoDA.FtpServer.Models
{
    public class FileModel
    {
        public string Name { get; }
        public DateTime LastWriteTime { get; }
        public long Length { get; }
        public FileModel(string name, DateTime lastWriteTime, long length)
        {
            Name = name;
            LastWriteTime = lastWriteTime;
            Length = length;
        }
    }
}
