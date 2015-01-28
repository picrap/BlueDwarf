
namespace BlueDwarf.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Collection;
    using Configuration;
    using Microsoft.Practices.Unity;
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

        [NotifyPropertyChanged]
        public IList<ProxyPageProvider> ProxyPageProviders { get; set; }

        private ProxyPageProvider _proxyPageProvider;

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

        public ProxyPickerViewModel()
        {
            Locale = new ProxyPickerLocale();
            ProxyServers = new DispatcherObservableCollection<HostPort>();
            ProxyPageProviders = ProxyPageProvider.Default;
            ProxyPageProvider = ProxyPageProviders[0];
        }

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
