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
    [DebuggerDisplay("Server: {EndPoint}")]
    public class HostEndPoint : DnsEndPoint
    {
        // A literal decimal byte, from 0 to 255
        private const string ByteRangeEx = @"(\d{1,2}|([0-1]\d{2})|(2[0-4]\d)|(25[0-5]))";
        // so an IP v4 is 4 literal bytes, separated by a dot
        public const string IPv4Ex = @"(?<address>(" + ByteRangeEx + @"\." + ByteRangeEx + @"\." + ByteRangeEx + @"\." + ByteRangeEx + @"))";
        // A port from 0 to 65535
        public const string PortEx = @"(?<port>(([0-5]\d{4})|(6[0-4]\d{3})|(65[0-4]\d{2})|(655[0-2]\d)|(6553[0-5])|\d{1,4}))";

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
