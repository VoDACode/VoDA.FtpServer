using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoDA.FtpServer.Extensions
{
    internal static class StreamExtension
    {
        public static long CopyToStream(this Stream input, Stream output, int bufferSize = 4096)
        {
            byte[] buffer = new byte[bufferSize];
            int count = 0;
            long total = 0;
            while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, count);
                total += count;
            }
            return total;
        }
    }
}
