using System.IO;
using System.Text;

namespace BlueDwarf.IO
{
    /// <summary>
    /// Exntensions to Stream
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads all available data.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static int ReadAll(this Stream stream, byte[] buffer, int offset, int count)
        {
            var totalRead = 0;
            while (count > 0)
            {
                var bytesRead = stream.Read(buffer, offset, count);
                if (bytesRead == 0)
                    break;

                totalRead += bytesRead;
                offset += bytesRead;
                count -= bytesRead;
            }
            return totalRead;
        }

        /// <summary>
        /// Reads the line as ASCII.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static string ReadLineAscii(this Stream stream)
        {
            var lineBuilder = new StringBuilder();
            for (; ; )
            {
                var b = stream.ReadByte();
                if (b == -1)
                    break;
                var c = (char)b;
                if (c == '\r')
                    continue;
                if (c == '\n')
                    break;
                lineBuilder.Append(c);
            }
            return lineBuilder.ToString();
        }
    }
}