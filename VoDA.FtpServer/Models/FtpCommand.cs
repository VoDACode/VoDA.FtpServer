using System.IO;
using System.Linq;
using System.Text;

namespace VoDA.FtpServer.Models
{
    internal class FtpCommand
    {
        private static readonly string[] _securityCommands = { "PASS" };

        public FtpCommand(string line)
        {
            var tmp = line.Split(' ');
            Command = tmp[0].ToUpperInvariant();
            Arguments = tmp.Length > 1 ? line.Substring(tmp[0].Length + 1) : null;
            if (Arguments != null)
                Arguments = Arguments.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (string.IsNullOrWhiteSpace(Arguments))
                Arguments = null;
        }

        public string Command { get; }
        public string? Arguments { get; }

        public override string ToString()
        {
            if (!_securityCommands.Any(p => p == Command) || Arguments == null)
                return $"{Command} {Arguments}";
            var str = new StringBuilder(Arguments.Length);
            str.Append(Command);
            str.Append(' ');
            for (var i = 0; i < Arguments.Length; i++)
                if (Arguments[i] == ' ')
                    str.Append(' ');
                else
                    str.Append('*');
            return str.ToString();
        }
    }
}