// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using Utility;

    /// <summary>
    /// Represents a route to target
    /// This class holds the router instance
    /// </summary>
    public class Route
    {
        public delegate Socket ConnectHostDelegate(string targetHost, int targetPort, Route route);
        public delegate Socket ConnectAddressDelegate(IPAddress address, int targetPort, Route route);

        /// <summary>
        /// Gets the relays (the proxy servers tunnel).
        /// </summary>
        /// <value>
        /// The relays.
        /// </value>
        internal ProxyServer[] Relays { get; private set; }

        private readonly ConnectHostDelegate _connectHost;
        private readonly ConnectAddressDelegate _connectAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="Route" /> class.
        /// </summary>
        /// <param name="connectHost">The connect host.</param>
        /// <param name="connectAddress">The connect address.</param>
        /// <param name="route">The route.</param>
        public Route(ConnectHostDelegate connectHost, ConnectAddressDelegate connectAddress, params ProxyServer[] route)
        {
            _connectHost = connectHost;
            _connectAddress = connectAddress;
            Relays = route.Where(r => r != null).ToArray();
        }

        /// <summary>
        /// Adds an element to current route and returns a new route.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="proxyServer">The proxy server.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Route operator +(Route route, ProxyServer proxyServer)
        {
            return new Route(route._connectHost, route._connectAddress, route.Relays.Concat(new[] { proxyServer }).ToArray());
        }

        /// <summary>
        /// Connects to specified target host/port.
        /// </summary>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <returns></returns>
        public Socket Connect(string targetHost, int targetPort)
        {
            return _connectHost(targetHost, targetPort, this);
        }

        /// <summary>
        /// Connects the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="targetPort">The target port.</param>
        /// <returns></returns>
        public Socket Connect(IPAddress target, int targetPort)
        {
            return _connectAddress(target, targetPort, this);
        }

        /// <summary>
        /// Connects the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public Stream Connect(Uri uri)
        {
            Stream stream = Connect(uri.Host, uri.Port).ToNetworkStream();
            if (stream != null && string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.InvariantCultureIgnoreCase))
                stream = stream.AsSsl(uri.Host);
            return stream;
        }
    }
}