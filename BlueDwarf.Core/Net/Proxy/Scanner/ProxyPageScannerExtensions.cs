// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using System.Collections.Generic;

    public static class ProxyPageScannerExtensions
    {
        /// <summary>
        /// Scans a given page for proxy servers.
        /// </summary>
        /// <param name="proxyPageScanner">The proxy page scanner.</param>
        /// <param name="proxyPageProvider">The proxy page provider.</param>
        /// <param name="testTargetHost">The test target host.</param>
        /// <param name="testTargetPort">The test target port.</param>
        /// <param name="proxyServers">The proxy servers.</param>
        /// <returns></returns>
        public static IEnumerable<HostPort> ScanPage(this IProxyPageScanner proxyPageScanner, ProxyPageProvider proxyPageProvider, string testTargetHost, int testTargetPort, params  Uri[] proxyServers)
        {
            return proxyPageScanner.ScanPage(proxyPageProvider.PageUri, proxyPageProvider.ParseAsText, proxyPageProvider.HostPortEx, testTargetHost, testTargetPort, proxyServers);
        }
    }
}
