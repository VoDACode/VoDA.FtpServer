using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using VoDA.FtpServer.Enums;

namespace VoDA.FtpServer.Extensions
{
    internal static class StreamExtension
    {
        public static long CopyToStream(this Stream input, Stream output, int bufferSize, TransferType transferType, CancellationToken token, long startIndex = 0)
        {
            int count = 0;
            long total = 0;
            input.Seek(startIndex, SeekOrigin.Begin);
            if (transferType == TransferType.Image)
            {
                byte[] buffer = new byte[bufferSize];
                while (!token.IsCancellationRequested && (count = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    try
                    {
                        output.Write(buffer, 0, count);
                        output.Flush();
                        total += count;
                    }
                    catch 
                    {
                        break;
                    }
                }
            }
            else
            {
                char[] buffer = new char[bufferSize];
                using (StreamReader rdr = new StreamReader(input, Encoding.ASCII))
                {
                    using (StreamWriter wtr = new StreamWriter(output, Encoding.ASCII))
                    {
                        while (!token.IsCancellationRequested && (count = rdr.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            wtr.Write(buffer, 0, count);
                            total += count;
                        }
                    }
                }
            }
            return total;
        }
    }
}
