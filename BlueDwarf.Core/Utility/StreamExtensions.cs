// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Utility
{
    using System.IO;
    using System.Net.Security;

    /// <summary>
    /// Extensions to Stream
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Opens SSL channel.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="targetHost">The target host.</param>
        /// <returns></returns>
        public static Stream AsSsl(this Stream stream, string targetHost)
        {
            if (stream is SslStream)
                return stream;

            var sslStream = new SslStream(stream);
            sslStream.AuthenticateAsClient(targetHost);
            return sslStream;
        }
    }
}
