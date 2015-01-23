// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Annotations;
    using Client;
    using Microsoft.Practices.Unity;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    internal class ProxyScanner : IProxyScanner
    {
        [Dependency]
        public IDownloader Downloader { get; set; }

        [Dependency]
        public IHostScanner HostScanner { get; set; }

        [Dependency]
        public IProxyValidator ProxyValidator { get; set; }

        public IEnumerable<HostPort> ScanPage(Uri proxyListingPage, string targetHost, int targetPort, ProxyRoute proxyRoute)
        {
            var proxyListingPageText = Downloader.DownloadText(proxyListingPage, proxyRoute);
            if (proxyListingPageText == null)
                return new HostPort[0];
            return HostScanner.Scan(proxyListingPageText).AsParallel().Where(hp => ProxyValidator.Validate(hp, targetHost, targetPort, proxyRoute));
        }
    }
}
