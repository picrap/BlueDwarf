// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.ViewModel
{
    using Net.Geolocation;
    using Net.Proxy.Scanner;

    public class Proxy
    {
        public HostPort HostPort { get; private set; }

        public AddressGeolocation Geolocation { get; private set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get { return HostPort.HostOrAddress + ":" + HostPort.Port; } }

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
        /// <param name="hostPort">The host port.</param>
        /// <param name="geolocation">The geolocation.</param>
        /// <param name="pingMs">The ping (in ms).</param>
        /// <param name="speedKbps">The speed (in kB/s).</param>
        public Proxy(HostPort hostPort, AddressGeolocation geolocation, int pingMs, int speedKbps)
        {
            HostPort = hostPort;
            Geolocation = geolocation;
            PingMs = pingMs;
            SpeedKbps = speedKbps;
        }
    }
}
