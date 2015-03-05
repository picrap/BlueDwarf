// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Annotations;
    using Client;
    using Microsoft.Practices.Unity;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    internal class ProxyPageScanner : IProxyPageScanner
    {
        [Dependency]
        public IDownloader Downloader { get; set; }

        [Dependency]
        public IHostScanner HostScanner { get; set; }

        [Dependency]
        public IProxyValidator ProxyValidator { get; set; }

        /// <summary>
        /// Scans a given page for proxy servers.
        /// </summary>
        /// <param name="proxyListingPage">The proxy listing page.</param>
        /// <param name="parseAsRawText">If the page must be parsed as raw text (no HTML tags)</param>
        /// <param name="hostPortEx">The custom parsing expression (providing "address" or "host" and "port" tags) or null to use default</param>
        /// <param name="route"></param>
        /// <param name="testTarget"></param>
        /// <returns></returns>
        public IEnumerable<HostPort> ScanPage(Uri proxyListingPage, bool parseAsRawText, string hostPortEx, Route route, Uri testTarget)
        {
            var proxyListingPageText = Downloader.Download(proxyListingPage, parseAsRawText, route);
            if (proxyListingPageText == null)
                return new HostPort[0];
            return HostScanner.Scan(proxyListingPageText, hostPortEx)/*.AsParallel().WithDegreeOfParallelism(63)*/.Where(hp => ProxyValidator.ValidateHttpConnect(hp, testTarget, route));
        }

        /// <summary>
        /// Scans a given page for proxy servers.
        /// </summary>
        /// <param name="hostPorts">The host ports.</param>
        /// <param name="proxyListingPage">The proxy listing page.</param>
        /// <param name="parseAsRawText">if set to <c>true</c> [parse as raw text].</param>
        /// <param name="hostPortEx">The host port ex.</param>
        /// <param name="route"></param>
        /// <param name="testTarget"></param>
        public void ScanPage(IList<HostPort> hostPorts, Uri proxyListingPage, bool parseAsRawText, string hostPortEx, Route route, Uri testTarget)
        {
            var proxyListingPageText = Downloader.Download(proxyListingPage, parseAsRawText, route);
            if (proxyListingPageText != null)
            {
                // All runs as parallel, since this is a massive network check with no CPU load at all
                // TODO: parallelize more
                HostScanner.Scan(proxyListingPageText, hostPortEx).AsParallel().WithDegreeOfParallelism(63).ForAll(
                       delegate(HostPort hp)
                       {
                           if (ProxyValidator.ValidateHttpConnect(hp, testTarget, route))
                           {
                               lock (hostPorts)
                                   hostPorts.Add(hp);
                           }
                       });
            }
        }
    }
}
