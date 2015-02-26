// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Collection;
    using Configuration;
    using Microsoft.Practices.Unity;
    using Navigation;
    using Net.Geolocation;
    using Net.Proxy.Client;
    using Net.Proxy.Scanner;
    using Properties;
    using Resources.Localization;
    using Utility;

    public class ProxyPickerViewModel : ViewModel
    {
        [Dependency]
        public IProxyPageScanner ProxyPageScanner { get; set; }

        [Dependency]
        public IPersistence Persistence { get; set; }

        [Dependency]
        public INavigator Navigator { get; set; }

        [Dependency]
        public IGeolocation Geolocation { get; set; }

        [Dependency]
        public IProxyClient ProxyClient { get; set; }

        [NotifyPropertyChanged]
        public IList<ProxyPage> ProxyPages { get; set; }

        private ProxyPage _proxyPage;

        [Persistent("ProxyPage", AutoSave = true)]
        public Uri ProxyPageUri { get; set; }

        /// <summary>
        /// Gets or sets the selected proxy page.
        /// </summary>
        /// <value>
        /// The proxy page.
        /// </value>
        [NotifyPropertyChanged]
        public ProxyPage ProxyPage
        {
            get { return _proxyPage; }
            set
            {
                _proxyPage = value;
                ProxyPageUri = value.PageUri;
                CheckProxyServers();
            }
        }

        /// <summary>
        /// Gets or sets the proxy servers list. Selector is bound to this
        /// </summary>
        /// <value>
        /// The proxy servers.
        /// </value>
        public IList<Proxy> ProxyServers { get; set; }

        [Persistent("ProxyTest", AutoSave = true)]
        public string TestTarget { get; set; }

        /// <summary>
        /// Gets the test target URI.
        /// </summary>
        /// <value>
        /// The test target URI.
        /// </value>
        public Uri TestTargetUri
        {
            get
            {
                try
                {
                    if (TestTarget != null)
                        return new Uri(TestTarget);
                }
                catch (UriFormatException)
                {
                }
                return null;
            }
        }

        [Persistent("Proxy1")]
        public Uri LocalProxy { get; set; }

        public ProxyPickerLocale Locale { get; set; }

        private Proxy _proxyServer;

        /// <summary>
        /// Gets or sets the selected proxy server.
        /// If non null, this causes the window to close
        /// </summary>
        /// <value>
        /// The proxy server.
        /// </value>
        [NotifyPropertyChanged]
        public Proxy ProxyServer
        {
            get { return _proxyServer; }
            set
            {
                _proxyServer = value;
                Navigator.Exit(true);
            }
        }

        /// <summary>
        /// Loads data related to this view-model.
        /// </summary>
        public override void Load()
        {
            Locale = new ProxyPickerLocale();
            ProxyServers = new DispatcherObservableCollection<Proxy>();
            ProxyPages = ProxyPage.Default;
            ProxyPage = ProxyPages.SingleOrDefault(p => p.PageUri == ProxyPageUri) ?? ProxyPages[0];
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            Navigator.Exit(false);
        }

        /// <summary>
        /// Scans for proxy servers in given page.
        /// </summary>
        [Async(KillExisting = true)]
        private void CheckProxyServers()
        {
            ProxyServers.Clear();
            var testTargetUri = TestTargetUri;
            if (testTargetUri != null)
            {
                var proxyRoute = ProxyClient.SafeCreateRoute(testTargetUri.Host, testTargetUri.Port, LocalProxy);
                if (proxyRoute == null)
                    return;
                foreach (var hostPort in ProxyPageScanner.ScanPage(ProxyPage.PageUri, ProxyPage.ParseAsText, ProxyPage.HostPortEx, testTargetUri.Host, testTargetUri.Port, LocalProxy))
                {
                    var location = Geolocation.Locate(hostPort.Address, proxyRoute);
                    var proxy = new Proxy(hostPort, location);
                    ProxyServers.Add(proxy);
                }
            }
        }
    }
}
