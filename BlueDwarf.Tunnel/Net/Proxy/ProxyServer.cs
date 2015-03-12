// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Represents a proxy server
    /// </summary>
    [DebuggerDisplay("{Protocol}=[{Address}]:{Port}")]
    public class ProxyServer : IPEndPoint
    {
        /// <summary>
        /// Gets the protocol.
        /// </summary>
        /// <value>
        /// The protocol.
        /// </value>
        public ProxyProtocol Protocol { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyServer"/> class.
        /// </summary>
        /// <param name="protocol">The protocol.</param>
        /// <param name="address">The address.</param>
        /// <param name="port">The port.</param>
        public ProxyServer(ProxyProtocol protocol, IPAddress address, int port)
            : base(address, port)
        {
            Protocol = protocol;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyServer"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        public ProxyServer(Uri uri)
            : base(GetIPAddress(uri), GetPort(uri))
        {
            Protocol = GetProtocol(uri.Scheme);
        }

        /// <summary>
        /// Gets the ip address.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        private static IPAddress GetIPAddress(Uri uri)
        {
            IPAddress ipAddress;
            if (IPAddress.TryParse(uri.Host, out ipAddress))
                return ipAddress;
            // if it fails here, there's nothing we can do here
            var entry = Dns.GetHostEntry(uri.Host);
            return entry.AddressList.First(a => a.AddressFamily == AddressFamily.InterNetwork)
                   ?? entry.AddressList.First(a => a.AddressFamily == AddressFamily.InterNetworkV6);
        }

        /// <summary>
        /// Gets the port.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        private static int GetPort(Uri uri)
        {
            var port = uri.Port;
            if (port > 0)
                return port;
            if (uri.Scheme == "socks")
                return 1080;
            return 0;
        }

        /// <summary>
        /// Gets the protocol.
        /// </summary>
        /// <param name="scheme">The scheme.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private static ProxyProtocol GetProtocol(string scheme)
        {
            switch (scheme)
            {
                case "http":
                case "https":
                    return ProxyProtocol.HttpConnect;
                case "socks":
                case "socks4":
                    return ProxyProtocol.Socks4;
                default:
                    throw new ArgumentOutOfRangeException(scheme);
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Uri"/> to <see cref="ProxyServer"/>.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ProxyServer(Uri uri)
        {
            if (uri == null)
                return null;
            return new ProxyServer(uri);
        }
    }
}
