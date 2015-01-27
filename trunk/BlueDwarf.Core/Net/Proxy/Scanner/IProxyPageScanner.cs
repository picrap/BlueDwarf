// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Scans pages for proxy servers and validates them
    /// </summary>
    public interface IProxyPageScanner
    {
        /// <summary>
        /// Scans a given page for proxy servers.
        /// </summary>
        /// <param name="proxyListingPage">The proxy listing page.</param>
        /// <param name="parseAsRawText">If the page must be parsed as raw text (no HTML tags)</param>
        /// <param name="hostPortEx">The custom parsing expression (providing "address" or "host" and "port" tags) or null to use default</param>
        /// <param name="testTargetHost"></param>
        /// <param name="testTargetPort"></param>
        /// <param name="proxyServers"></param>
        /// <returns></returns>
        IEnumerable<HostPort> ScanPage(Uri proxyListingPage, bool parseAsRawText, string hostPortEx, string testTargetHost, int testTargetPort, params Uri[] proxyServers);

        /// <summary>
        /// Scans a given page for proxy servers.
        /// </summary>
        /// <param name="hostPorts">The host ports.</param>
        /// <param name="proxyListingPage">The proxy listing page.</param>
        /// <param name="parseAsRawText">if set to <c>true</c> [parse as raw text].</param>
        /// <param name="hostPortEx">The host port ex.</param>
        /// <param name="testTargetHost">The test target host.</param>
        /// <param name="testTargetPort">The test target port.</param>
        /// <param name="proxyServers">The proxy servers.</param>
        void ScanPage(IList<HostPort> hostPorts, Uri proxyListingPage, bool parseAsRawText, string hostPortEx, string testTargetHost, int testTargetPort, params Uri[] proxyServers);
    }
}