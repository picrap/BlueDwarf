// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using System.Collections.Generic;
    using Client;

    public interface IProxyScanner
    {
        /// <summary>
        /// Scans a given page for proxy servers.
        /// </summary>
        /// <param name="proxyListingPage">The proxy listing page.</param>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="proxyRoute">The proxy route.</param>
        /// <returns></returns>
        IEnumerable<HostPort> ScanPage(Uri proxyListingPage, string targetHost, int targetPort, ProxyRoute proxyRoute);
    }
}