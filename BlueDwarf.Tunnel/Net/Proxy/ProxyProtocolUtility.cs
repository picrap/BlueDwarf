// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Proxy
{
    using System;

    /// <summary>
    /// Utility class (utility and extensions) for <see cref="ProxyProtocol"/>
    /// </summary>
    public static class ProxyProtocolUtility
    {
        /// <summary>
        /// Converts a literal string to a protocol.
        /// </summary>
        /// <param name="literalProtocol">The literal protocol.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">literalProtocol</exception>
        public static ProxyProtocol FromLiteral(string literalProtocol)
        {
            switch (literalProtocol.ToLower())
            {
                case "http":
                    return ProxyProtocol.HttpConnect;
                case "socks":
                case "socks4":
                case "socks4a":
                    return ProxyProtocol.Socks4A;
                default:
                    throw new ArgumentOutOfRangeException("literalProtocol");
            }
        }

        /// <summary>
        /// Converts a protocol to a literal.
        /// </summary>
        /// <param name="proxyProtocol">The proxy protocol.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">proxyProtocol</exception>
        public static string ToLiteral(this ProxyProtocol proxyProtocol)
        {
            switch (proxyProtocol)
            {
                case ProxyProtocol.HttpConnect:
                    return "http";
                case ProxyProtocol.Socks4A:
                    return "socks";
                default:
                    throw new ArgumentOutOfRangeException("proxyProtocol");
            }
        }
    }
}
