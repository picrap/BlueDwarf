// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client
{
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
        /// <summary>
        /// Method prototype to connect to given target (IP+port) and using given route
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        public delegate Socket ConnectAddressDelegate(IPAddress address, int targetPort, Route route);

        /// <summary>
        /// Gets the relays (the proxy servers tunnel).
        /// </summary>
        /// <value>
        /// The relays.
        /// </value>
        public ProxyServer[] Relays { get; private set; }

        private readonly ConnectAddressDelegate _connectAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="Route" /> class.
        /// </summary>
        /// <param name="connectAddress">The connect address.</param>
        /// <param name="route">The route.</param>
        public Route(ConnectAddressDelegate connectAddress, params ProxyServer[] route)
        {
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
            return new Route(route._connectAddress, route.Relays.Concat(new[] { proxyServer }).ToArray());
        }

        /// <summary>
        /// Connects to the specified target.
        /// </summary>
        /// <param name="targetAddress">The target.</param>
        /// <param name="targetPort">The target port.</param>
        /// <returns></returns>
        public Socket Connect(IPAddress targetAddress, int targetPort)
        {
            return _connectAddress(targetAddress, targetPort, this);
        }

        /// <summary>
        /// Connects to the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public Socket Connect(EndPoint target)
        {
            var ipTarget = target.ToIPEndPoint();
            return Connect(ipTarget.Address, ipTarget.Port);
        }
    }
}