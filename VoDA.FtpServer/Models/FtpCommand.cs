namespace VoDA.FtpServer.Models
{
    internal class FtpCommand
    {
        public string Command { get; }
        public string? Arguments { get; }
        public FtpCommand(string line)
        {
            var tmp = line.Split(' ');
            Command = tmp[0].ToUpperInvariant();
            Arguments = tmp.Length > 1 ? line.Substring(tmp[0].Length + 1) : null;
            if (string.IsNullOrWhiteSpace(Arguments))
                Arguments = null;
        }

        public override string ToString()
        {
            return $"{Command} {Arguments}";
        }
    }
}
