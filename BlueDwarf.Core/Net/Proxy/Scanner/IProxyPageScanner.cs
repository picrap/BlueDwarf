// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using System.Collections.Generic;
    using Client;

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
        /// <param name="route"></param>
        /// <param name="testTarget"></param>
        /// <returns></returns>
        IEnumerable<ProxyServer> ScanPage(Uri proxyListingPage, bool parseAsRawText, string hostPortEx, Route route, Uri testTarget);

        /// <summary>
        /// Scans a given page for proxy servers.
        /// </summary>
        /// <param name="proxyServers"></param>
        /// <param name="proxyListingPage">The proxy listing page.</param>
        /// <param name="parseAsRawText">if set to <c>true</c> [parse as raw text].</param>
        /// <param name="hostPortEx">The host port ex.</param>
        /// <param name="route"></param>
        /// <param name="testTarget"></param>
        void ScanPage(IList<ProxyServer> proxyServers, Uri proxyListingPage, bool parseAsRawText, string hostPortEx, Route route, Uri testTarget);
    }
}