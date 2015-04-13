// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Proxy
{
    using System;

    partial class ProxyServer
    {
        /// <summary>
        /// Parses the literal value.
        /// Accepted formats are protocol=host:port or protocol://host:port
        /// </summary>
        /// <param name="literalValue">The literal value.</param>
        /// <returns></returns>
        private static Uri ParseLiteral(string literalValue)
        {
            return ParseLiteralProxy(literalValue) ?? ParseLiteralUri(literalValue);
        }

        /// <summary>
        /// Parses the literal in Windows proxy form (protocol=host:port).
        /// </summary>
        /// <param name="literalValue">The literal value.</param>
        /// <returns></returns>
        private static Uri ParseLiteralProxy(string literalValue)
        {
            var index = literalValue.IndexOf('=');
            if (index < 0)
                return null;
            var scheme = literalValue.Substring(0, index);
            var hostAndPort = literalValue.Substring(index + 1);
            // and now for the lazy part
            return ParseLiteralUri(string.Format("{0}://{1}", scheme, hostAndPort));
        }

        /// <summary>
        /// Parses the literal URI (protocol://host:port).
        /// </summary>
        /// <param name="literalValue">The literal value.</param>
        /// <returns></returns>
        private static Uri ParseLiteralUri(string literalValue)
        {
            try
            {
                return new Uri(literalValue);
            }
            catch (UriFormatException)
            { }
            return null;
        }

        /// <summary>
        /// Gets the host from literal.
        /// </summary>
        /// <param name="literalValue">The literal value.</param>
        /// <returns></returns>
        private static string GetHostFromLiteral(string literalValue)
        {
            var uri = ParseLiteral(literalValue);
            if (uri == null)
                return null;
            return uri.Host;
        }

        /// <summary>
        /// Gets the port from literal.
        /// </summary>
        /// <param name="literalValue">The literal value.</param>
        /// <returns></returns>
        private static int GetPortFromLiteral(string literalValue)
        {
            var uri = ParseLiteral(literalValue);
            if (uri == null)
                return -1;
            return uri.Port;
        }

        /// <summary>
        /// Gets the protocol from literal.
        /// </summary>
        /// <param name="literalValue">The literal value.</param>
        /// <returns></returns>
        private static ProxyProtocol GetProtocolFromLiteral(string literalValue)
        {
            var uri = ParseLiteral(literalValue);
            if (uri == null)
                return ProxyProtocol.HttpConnect;
            return GetProtocol(uri.Scheme);
        }
    }
}
