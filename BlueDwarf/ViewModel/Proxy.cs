// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.ViewModel
{
    using System;
    using Net.Geolocation;
    using Net.Proxy;

    public class Proxy
    {
        /// <summary>
        /// Gets the proxy server.
        /// </summary>
        /// <value>
        /// The proxy server.
        /// </value>
        public ProxyServer ProxyServer { get; private set; }

        /// <summary>
        /// Gets the geolocation.
        /// </summary>
        /// <value>
        /// The geolocation.
        /// </value>
        public AddressGeolocation Geolocation { get; private set; }

        public string DisplayType
        {
            get
            {
                switch(ProxyServer.Protocol)
                {
                    case ProxyProtocol.HttpConnect:
                        return "HTTP";
                    case ProxyProtocol.Socks4A:
                        return "SOCKS";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get { return ProxyServer.Host + ":" + ProxyServer.Port; } }

        /// <summary>
        /// Gets the name of the country.
        /// </summary>
        /// <value>
        /// The name of the country.
        /// </value>
        public string CountryName { get { return Geolocation.CountryName; } }

        /// <summary>
        /// Gets the country code.
        /// </summary>
        /// <value>
        /// The country code.
        /// </value>
        public string CountryCode { get { return Geolocation.CountryCode; } }

        /// <summary>
        /// Gets the ping (in ms).
        /// </summary>
        /// <value>
        /// The ping ms.
        /// </value>
        public int PingMs { get; private set; }

        /// <summary>
        /// Gets the speed (in kB/s).
        /// </summary>
        /// <value>
        /// The speed KBPS.
        /// </value>
        public int SpeedKbps { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Proxy" /> class.
        /// </summary>
        /// <param name="proxyServer">The proxy server.</param>
        /// <param name="geolocation">The geolocation.</param>
        /// <param name="pingMs">The ping (in ms).</param>
        /// <param name="speedKbps">The speed (in kB/s).</param>
        public Proxy(ProxyServer proxyServer, AddressGeolocation geolocation, int pingMs, int speedKbps)
        {
            ProxyServer = proxyServer;
            Geolocation = geolocation;
            PingMs = pingMs;
            SpeedKbps = speedKbps;
        }
    }
}
