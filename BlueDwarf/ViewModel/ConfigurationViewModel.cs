// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.ViewModel
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using Annotations;
    using Configuration;
    using Controls;
    using Microsoft.Practices.Unity;
    using Navigation;
    using Net.Proxy.Client;
    using Net.Proxy.Server;
    using Properties;
    using Utility;

    /// <summary>
    /// Configuration view-model.
    /// This is the main view
    /// </summary>
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class ConfigurationViewModel : ViewModel
    {
        [Dependency]
        public IProxyClient ProxyClient { get; set; }

        [Dependency]
        public IProxyServer ProxyServer { get; set; }

        [Dependency]
        public INavigator Navigator { get; set; }

        [Dependency]
        public IPersistence Persistence { get; set; }

        public enum Category
        {
            None = 0,
            ProxyTunnel,
            ProxyServer,
            ProxyKeepalive,
        }

        [Persistent("Proxy1")]
        [NotifyPropertyChanged(Category = Category.ProxyTunnel)]
        public Uri LocalProxy { get; set; }

        [NotifyPropertyChanged]
        public StatusCode LocalProxyStatus { get; set; }

        [Persistent("Proxy2")]
        [NotifyPropertyChanged(Category = Category.ProxyTunnel)]
        public Uri RemoteProxy { get; set; }

        [NotifyPropertyChanged]
        public StatusCode RemoteProxyStatus { get; set; }

        [Persistent("ProxyTest", Default = "https://google.com")]
        [NotifyPropertyChanged(Category = Category.ProxyTunnel)]
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

        [NotifyPropertyChanged]
        public StatusCode TestTargetStatus { get; set; }

        [Persistent("KeepAlive1", Default = "https://google.com")]
        [NotifyPropertyChanged(Category = Category.ProxyKeepalive)]
        public Uri KeepAlive1 { get; set; }

        [Persistent("KeepAlive1Interval", Default = 120)]
        [NotifyPropertyChanged(Category = Category.ProxyKeepalive)]
        public int KeepAlive1Interval { get; set; }

        [NotifyPropertyChanged]
        public Uri KeepAlive1FullUri { get; set; }

        [Persistent("KeepAlive2")]
        [NotifyPropertyChanged(Category = Category.ProxyKeepalive)]
        public Uri KeepAlive2 { get; set; }

        [Persistent("KeepAlive2Interval", Default = 120)]
        [NotifyPropertyChanged(Category = Category.ProxyKeepalive)]
        public int KeepAlive2Interval { get; set; }

        [NotifyPropertyChanged]
        public Uri KeepAlive2FullUri { get; set; }

        [NotifyPropertyChanged(Category = Category.ProxyServer)]
        public int SocksListeningPort { get; set; }

        [Persistent("SocksListeningPort", Default = 1080)]
        public int PersistentSocksListeningPort { get; set; }

        private bool _canSetSocksListeningPort = true;
        [NotifyPropertyChanged]
        public bool CanSetSocksListeningPort
        {
            get { return _canSetSocksListeningPort; }
            set { _canSetSocksListeningPort = value; }
        }

        [NotifyPropertyChanged]
        public bool Show { get; set; }

        [NotifyPropertyChanged]
        public int ConnectionsCount { get; set; }

        [NotifyPropertyChanged]
        public long BytesRead { get; set; }

        [NotifyPropertyChanged]
        public long BytesWritten { get; set; }

        private readonly object _statisticsLock = new object();

        /// <summary>
        /// Loads preferences and intializes proxy from them.
        /// </summary>
        public override void Load()
        {
            if (CanSetSocksListeningPort)
                SocksListeningPort = PersistentSocksListeningPort;
            PropertyChanged += OnPropertyChanged;
            ProxyServer.Connect += OnProxyServerConnect;
            ProxyServer.Transfer += OnProxyServerTransfer;
            CheckProxyTunnel();
            SetupProxyServer();
            KeepAlive(() => KeepAlive1, () => KeepAlive1Interval, u => KeepAlive1FullUri = u);
            KeepAlive(() => KeepAlive2, () => KeepAlive2Interval, u => KeepAlive2FullUri = u);
        }

        private void OnProxyServerConnect(object sender, EventArgs e)
        {
            lock (_statisticsLock)
                ConnectionsCount++;
        }

        private void OnProxyServerTransfer(object sender, ProxyServerTransferEventArgs e)
        {
            lock (_statisticsLock)
            {
                BytesRead += e.BytesRead;
                BytesWritten += e.BytesWritten;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var category = e.GetCategory<Category>();
            if (category != Category.None)
                UpdatePreferences();
            switch (category)
            {
                case Category.ProxyTunnel:
                    CheckProxyTunnel();
                    break;
                case Category.ProxyServer:
                    SetupProxyServer();
                    break;
                case Category.ProxyKeepalive:
                    break;
            }
        }

        /// <summary>
        /// Updates the preferences.
        /// </summary>
        private void UpdatePreferences()
        {
            if (CanSetSocksListeningPort)
                PersistentSocksListeningPort = SocksListeningPort;
            Persistence.Write();
        }

        [Async(KillExisting = true)]
        private void CheckProxyTunnel()
        {
            try
            {
                SetStatusPending();
                var testTargetUri = TestTargetUri;
                if (testTargetUri != null)
                    ProxyServer.ProxyRoute = ProxyClient.CreateRoute(testTargetUri.Host, testTargetUri.Port, LocalProxy, RemoteProxy);
                SetStatus(null);
            }
            catch (ProxyRouteException pre)
            {
                ProxyServer.ProxyRoute = null;
                SetStatus(pre);
            }
        }

        private readonly object _keepAliveSerialLock = new object();
        private int _keepAliveSerial;

        [Async]
        private void KeepAlive(Func<Uri> getBaseUri, Func<int> getInterval, Action<Uri> setFullUri)
        {
            for (; ; )
            {
                var baseUri = getBaseUri();
                if (baseUri != null)
                {
                    lock (_keepAliveSerialLock)
                    {
                        var randomParameter = string.Format("whatthefook={0}", ++_keepAliveSerial);
                        var uri = baseUri.Query.IsNullOrEmpty()
                            ? new Uri(baseUri + "?" + randomParameter)
                            : new Uri(baseUri + "&" + randomParameter);
                        setFullUri(uri);
                    }
                }

                var interval = Math.Max(getInterval(), 10);
                Thread.Sleep(interval * 1000);
            }
        }

        private void SetStatusPending()
        {
            LocalProxyStatus = StatusCode.None;
            RemoteProxyStatus = StatusCode.None;
            TestTargetStatus = StatusCode.None;
        }

        private void SetStatus(ProxyRouteException proxyRouteException)
        {
            Func<Uri, bool> checkUri = u => proxyRouteException != null && proxyRouteException.Proxy == u;
            Func<string, bool> checkHost = h => proxyRouteException != null && proxyRouteException.TargetHost == h;
            var testTargetUri = TestTargetUri;
            SetStatusLines(
                Tuple.Create<Func<bool>, Action<StatusCode>>(() => checkUri(LocalProxy), v => LocalProxyStatus = LocalProxy != null ? v : StatusCode.None),
                Tuple.Create<Func<bool>, Action<StatusCode>>(() => checkUri(RemoteProxy), v => RemoteProxyStatus = RemoteProxy != null ? v : StatusCode.None),
                Tuple.Create<Func<bool>, Action<StatusCode>>(() => checkHost(testTargetUri != null ? TestTargetUri.Host : null), v => TestTargetStatus = v)
                );
        }

        private static void SetStatusLines(params Tuple<Func<bool>, Action<StatusCode>>[] failurePointsAndSetters)
        {
            var code = StatusCode.OK;
            foreach (var failurePointAndSetter in failurePointsAndSetters)
            {
                if (failurePointAndSetter.Item1())
                {
                    failurePointAndSetter.Item2(StatusCode.Error);
                    code = StatusCode.None;
                }
                else
                    failurePointAndSetter.Item2(code);
            }
        }

        private void SetupProxyServer()
        {
            ProxyServer.Port = SocksListeningPort;
        }

        /// <summary>
        /// Invokes the proxy analysis wizard (sort of).
        /// </summary>
        public void AnalyzeProxy()
        {
            var analysis = Navigator.Show<ProxyAnalysisViewModel>();
            if (analysis != null && analysis.DefaultProxy != null)
                LocalProxy = new Uri(analysis.DefaultProxy.ToString());
        }

        /// <summary>
        /// Minimizes to tray.
        /// </summary>
        public void Minimize()
        {
            Show = false;
        }

        /// <summary>
        /// Restores from tray.
        /// </summary>
        public void Restore()
        {
            Show = true;
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        public void Exit()
        {
            Navigator.Exit(true);
        }
    }
}
