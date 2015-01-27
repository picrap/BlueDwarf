// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Client
{
    using System;

    public class ProxyRouteException : Exception
    {
        /// <summary>
        /// Gets or sets the proxy where the connexion error happened.
        /// </summary>
        /// <value>
        /// The proxy.
        /// </value>
        public Uri Proxy { get; set; }

        /// <summary>
        /// If all proxy succeed but connection to target fails, the TargetHost is filled.
        /// </summary>
        /// <value>
        /// The target host.
        /// </value>
        public string TargetHost { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyRouteException"/> class.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        public ProxyRouteException(Uri proxy)
        {
            Proxy = proxy;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyRouteException"/> class.
        /// </summary>
        /// <param name="targetHost">The target host.</param>
        public ProxyRouteException(string targetHost)
        {
            TargetHost = targetHost;
        }
    }
}
