// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
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
        public IEnumerable<ProxyServer> ScanPage(Uri proxyListingPage, bool parseAsRawText, string hostPortEx, Route route, Uri testTarget)
        {
            string proxyListingPageText = null;
            try
            {
                proxyListingPageText = Downloader.Download(proxyListingPage, parseAsRawText, route);
            }
            catch (ProxyRouteException)
            { }
            if (proxyListingPageText == null)
                yield break;

            var results = new Queue<ProxyServer>();
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            var thread = new Thread(delegate()
            {
                HostScanner.Scan(proxyListingPageText, hostPortEx).AsParallel().WithDegreeOfParallelism(63)
                    .ForAll(delegate(ProxyServer proxyServer)
                    {
                        if (!ProxyValidator.ValidateHttpConnect(proxyServer, testTarget, route))
                            return;
                        lock (results)
                            results.Enqueue(proxyServer);
                        waitHandle.Set();
                    });
                lock (results)
                    results.Enqueue(null);
                waitHandle.Set();
            });
            thread.Start();
            for (; ; )
            {
                waitHandle.WaitOne();
                lock (results)
                {
                    while (results.Count > 0)
                    {
                        var result = results.Dequeue();
                        if (result == null)
                            yield break;
                        yield return result;
                    }
                }
            }
        }

        /// <summary>
        /// Scans a given page for proxy servers.
        /// </summary>
        /// <param name="proxyServers"></param>
        /// <param name="proxyListingPage">The proxy listing page.</param>
        /// <param name="parseAsRawText">if set to <c>true</c> [parse as raw text].</param>
        /// <param name="hostPortEx">The host port ex.</param>
        /// <param name="route"></param>
        /// <param name="testTarget"></param>
        public void ScanPage(IList<ProxyServer> proxyServers, Uri proxyListingPage, bool parseAsRawText, string hostPortEx, Route route, Uri testTarget)
        {
            var proxyListingPageText = Downloader.Download(proxyListingPage, parseAsRawText, route);
            if (proxyListingPageText != null)
            {
                // All runs as parallel, since this is a massive network check with no CPU load at all
                // TODO: parallelize more
                HostScanner.Scan(proxyListingPageText, hostPortEx).AsParallel().WithDegreeOfParallelism(63).ForAll(
                       delegate(ProxyServer proxyServer)
                       {
                           if (ProxyValidator.ValidateHttpConnect(proxyServer, testTarget, route))
                           {
                               lock (proxyServers)
                                   proxyServers.Add(proxyServer);
                           }
                       });
            }
        }
    }
}
