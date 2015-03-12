// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Proxy.Client
{
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Extensions to <see cref="Route"/>
    /// </summary>
    public static class RouteExtensions
    {
        /// <summary>
        /// Connects to target host/address and port.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="hostOrAddress">The host or address.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.IOException"></exception>
        public static Socket Connect(this Route route, string hostOrAddress, int port)
        {
            var entry = Dns.GetHostEntry(hostOrAddress);
            var ipAddress = entry.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork)
                            ?? entry.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetworkV6);
            if (ipAddress == null)
                throw new IOException(string.Format("Impossible to resolve '{0}'", hostOrAddress));
            return route.Connect(ipAddress, port);
        }
    }
}
