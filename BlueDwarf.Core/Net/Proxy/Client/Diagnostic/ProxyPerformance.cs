// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client.Diagnostic
{
    using System;

    public class ProxyPerformance
    {
        /// <summary>
        /// Gets the ping.
        /// </summary>
        /// <value>
        /// The ping.
        /// </value>
        public TimeSpan Ping { get; private set; }

        /// <summary>
        /// Gets the download speed (in bytes/s).
        /// </summary>
        /// <value>
        /// The download speed.
        /// </value>
        public double DownloadSpeed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyPerformance"/> class.
        /// </summary>
        /// <param name="ping">The ping.</param>
        /// <param name="downloadSpeed">The download speed.</param>
        public ProxyPerformance(TimeSpan ping, double downloadSpeed)
        {
            Ping = ping;
            DownloadSpeed = downloadSpeed;
        }
    }
}