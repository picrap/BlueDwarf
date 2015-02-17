// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Collection;
    using Configuration;
    using Microsoft.Practices.Unity;
    using Navigation;
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

        [NotifyPropertyChanged]
        public IList<ProxyPageProvider> ProxyPageProviders { get; set; }

        // static, because it is application-wide (and retrieved each time the panel is open)
        // TODO: save in preferences
        private static ProxyPageProvider _proxyPageProvider;

        [NotifyPropertyChanged]
        public ProxyPageProvider ProxyPageProvider
        {
            get { return _proxyPageProvider; }
            set
            {
                _proxyPageProvider = value;
                CheckProxyServers();
            }
        }

        public IList<HostPort> ProxyServers { get; set; }

        [Persistent("ProxyTest")]
        public string TestTarget { get; set; }

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

        private HostPort _proxyServer;

        /// <summary>
        /// Gets or sets the selected proxy server.
        /// If non null, this causes the window to close
        /// </summary>
        /// <value>
        /// The proxy server.
        /// </value>
        [NotifyPropertyChanged]
        public HostPort ProxyServer
        {
            get { return _proxyServer; }
            set
            {
                _proxyServer = value;
                Navigator.Exit(true);
            }
        }

        public ProxyPickerViewModel()
        {
            Locale = new ProxyPickerLocale();
            ProxyServers = new DispatcherObservableCollection<HostPort>();
            ProxyPageProviders = ProxyPageProvider.Default;
            if (ProxyPageProvider == null)
                ProxyPageProvider = ProxyPageProviders[0];
            else
                CheckProxyServers();
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
                ProxyPageScanner.ScanPage(ProxyServers, ProxyPageProvider.PageUri, ProxyPageProvider.ParseAsText, ProxyPageProvider.HostPortEx, testTargetUri.Host, testTargetUri.Port, LocalProxy);
        }
    }
}
