using System;
using System.IO;
using System.Text;
using System.Threading;
using VoDA.FtpServer.Enums;

namespace VoDA.FtpServer.Extensions
{
    internal static class StreamExtension
    {
        public static long CopyToStream(this Stream input, Stream output, int bufferSize, TransferType transferType,
            CancellationToken token, long startIndex = 0, Action<long, long>? progressEvent = null)
        {
            var count = 0;
            long total = 0;
            if (input.CanSeek)
                input.Seek(startIndex, SeekOrigin.Begin);
            if (transferType == TransferType.Image)
            {
                var buffer = new byte[bufferSize];
                while (!token.IsCancellationRequested && (count = input.Read(buffer, 0, buffer.Length)) > 0)
                    try
                    {
                        output.Write(buffer, 0, count);
                        output.Flush();
                        total += count;
                        progressEvent?.Invoke(input.CanSeek ? input.Length : 0, total);
                    }
                    catch
                    {
                        break;
                    }
            }
            else
            {
                var buffer = new char[bufferSize];
                using var readStream = new StreamReader(input, Encoding.ASCII);
                using var writeStream = new StreamWriter(output, Encoding.ASCII);
                while (!token.IsCancellationRequested &&
                       (count = readStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    writeStream.Write(buffer, 0, count);
                    total += count;
                    progressEvent?.Invoke(input.CanSeek ? input.Length : 0, total);
                }
            }

            return total;
        }
    }
}