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
        /// <param name="proxyPage">The proxy page provider.</param>
        /// <param name="testTargetHost">The test target host.</param>
        /// <param name="testTargetPort">The test target port.</param>
        /// <param name="proxyServers">The proxy servers.</param>
        /// <returns></returns>
        public static IEnumerable<HostPort> ScanPage(this IProxyPageScanner proxyPageScanner, ProxyPage proxyPage, string testTargetHost, int testTargetPort, params  Uri[] proxyServers)
        {
            return proxyPageScanner.ScanPage(proxyPage.PageUri, proxyPage.ParseAsText, proxyPage.HostPortEx, testTargetHost, testTargetPort, proxyServers);
        }
    }
}
