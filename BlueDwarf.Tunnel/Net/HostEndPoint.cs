// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Represents a server (host and port)
    /// </summary>
    [DebuggerDisplay("Server: {IPEndPoint}")]
    public class HostEndPoint : DnsEndPoint
    {
        private IPAddress _ipAddress;

        /// <summary>
        /// Gets the IPEndPoint (resolves Host if necessary).
        /// </summary>
        /// <value>
        /// The ip end point.
        /// </value>
        public IPEndPoint IPEndPoint
        {
            get
            {
                if (_ipAddress == null)
                    _ipAddress = ParseHost() ?? ResolveHost();
                return new IPEndPoint(_ipAddress, Port);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HostEndPoint" /> class.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="port">The port.</param>
        public HostEndPoint(IPAddress ipAddress, int port)
            : base(ipAddress.ToString(), port)
        {
            _ipAddress = ipAddress;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HostEndPoint"/> class.
        /// </summary>
        /// <param name="host">Nom d'hôte ou représentation sous forme de chaîne de l'adresse IP.</param>
        /// <param name="port">Numéro de port associé à l'adresse ou 0 pour spécifier tout port disponible.<paramref name="port" /> est dans l'ordre des hôtes.</param>
        public HostEndPoint(string host, int port)
            : base(host, port)
        {
        }

        /// <summary>
        /// Gets the ip address from the host.
        /// </summary>
        /// <returns></returns>
        private IPAddress ResolveHost()
        {
            try
            {
                var addresses = Dns.GetHostAddresses(Host);
                return addresses.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork)
                       ?? addresses.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetworkV6);
            }
            catch (SocketException) { }
            catch (ArgumentException) { }
            return null;
        }

        /// <summary>
        /// Parses the host.
        /// </summary>
        /// <returns></returns>
        private IPAddress ParseHost()
        {
            IPAddress ipAddress;
            IPAddress.TryParse(Host, out ipAddress);
            return ipAddress;
        }
    }
}
