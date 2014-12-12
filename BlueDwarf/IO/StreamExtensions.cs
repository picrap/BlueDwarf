using System.IO;
using System.Text;

namespace BlueDwarf.IO
{
    public static class StreamExtensions
    {
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

        public static string ReadLineASCII(this Stream stream)
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