// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client
{
    using System;
    using System.Linq;
    using Server;

    /// <summary>
    /// Represents a route to target
    /// This class holds the router instance
    /// </summary>
    public class Route
    {
        public delegate SocketStream ConnectDelegate(string targetHost, int targetPort, Route route, bool overrideDns);

        /// <summary>
        /// Gets the relays (the proxy servers tunnel).
        /// </summary>
        /// <value>
        /// The relays.
        /// </value>
        internal Uri[] Relays { get; private set; }

        private readonly ConnectDelegate _connect;

        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class.
        /// </summary>
        /// <param name="connect">The connect.</param>
        /// <param name="route">The route.</param>
        public Route(ConnectDelegate connect, params Uri[] route)
        {
            _connect = connect;
            Relays = route.Where(r => r != null).ToArray();
        }

        /// <summary>
        /// Adds an element to current route and returns a new route.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="uri">The URI.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Route operator +(Route route, Uri uri)
        {
            return new Route(route._connect, route.Relays.Concat(new[] { uri }).ToArray());
        }

        /// <summary>
        /// Connects to specified target host/port.
        /// </summary>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="overrideDns">if set to <c>true</c> resolve DNS by third party before connecting.</param>
        /// <returns></returns>
        public SocketStream Connect(string targetHost, int targetPort, bool overrideDns)
        {
            return _connect(targetHost, targetPort, this, overrideDns);
        }
    }
}