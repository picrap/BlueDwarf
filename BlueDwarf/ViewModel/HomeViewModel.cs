﻿// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using Annotations;
    using ArxOne.MrAdvice.MVVM.Navigation;
    using ArxOne.MrAdvice.MVVM.Properties;
    using ArxOne.MrAdvice.MVVM.Threading;
    using Configuration;
    using Controls;
    using Microsoft.Practices.Unity;
    using Net.Proxy;
    using Net.Proxy.Client;
    using Net.Proxy.Client.Diagnostic;
    using Net.Proxy.Scanner;
    using Net.Proxy.Server;
    using Properties;
    using Utility;

    /// <summary>
    /// Configuration view-model.
    /// This is the main view
    /// </summary>
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public partial class HomeViewModel : ViewModel
    {
        [Dependency]
        public IProxyClient ProxyClient { get; set; }

        [Dependency]
        public INavigator Navigator { get; set; }

        [Dependency]
        public IPersistence Persistence { get; set; }

        [Dependency]
        public IProxyValidator ProxyValidator { get; set; }

        [Dependency]
        public ISystemProxyAnalyzer SystemProxyAnalyzer { get; set; }

        [Dependency]
        public IProxyPageScanner ProxyPageScanner { get; set; }

        /// <summary>
        /// Occurs when route is updated.
        /// </summary>
        public event EventHandler RouteUpdated;

        /// <summary>
        /// Gets or sets the proxy server.
        /// This property is injected directly by the caller (the application)
        /// </summary>
        /// <value>
        /// The proxy server.
        /// </value>
        public IProxyServer ProxyServer { get; set; }

        private enum Category
        {
            None = 0,
            ProxyTunnel,
            ProxyServer,
            ProxyKeepalive,
        }

        /// <summary>
        /// Gets or sets the local proxy.
        /// The local proxy is the proxy that can be found in the LAN
        /// </summary>
        /// <value>
        /// The local proxy.
        /// </value>
        [Persistent("Proxy1")]
        [CategoryNotifyPropertyChanged(Category = Category.ProxyTunnel)]
        public Uri LocalProxy { get; set; }

        [NotifyPropertyChanged]
        public StatusCode LocalProxyStatus { get; set; }

        /// <summary>
        /// Gets or sets the remote proxy.
        /// The remote proxy, used with local proxy allows to access sites forbidden by the latter
        /// The remote proxy, used without local proxy allows to access sites anonymously (assuming the remote site is fully anonymous)
        /// </summary>
        /// <value>
        /// The remote proxy.
        /// </value>
        [Persistent("Proxy2")]
        [CategoryNotifyPropertyChanged(Category = Category.ProxyTunnel)]
        public Uri RemoteProxy { get; set; }

        [NotifyPropertyChanged]
        public StatusCode RemoteProxyStatus { get; set; }

        [Persistent("Proxy2Optional", DefaultValue = true)]
        [CategoryNotifyPropertyChanged(Category = Category.ProxyTunnel)]
        public bool OptionalRemoteProxy { get; set; }

        /// <summary>
        /// Gets or sets the test target.
        /// the blue dwarf tries to establish a connection, after going through local and remote proxy servers
        /// </summary>
        /// <value>
        /// The test target.
        /// </value>
        [Persistent("ProxyTest", DefaultValue = "https://isohunt.to/")]
        [CategoryNotifyPropertyChanged(Category = Category.ProxyTunnel)]
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

        /// <summary>
        /// Gets or sets the keep alive #1.
        /// Some proxy servers use an authentication, and de-authenticate when the internet access is idle for too long
        /// </summary>
        /// <value>
        /// The keep alive1.
        /// </value>
        [Persistent("KeepAlive1", DefaultValue = "https://google.com")]
        [CategoryNotifyPropertyChanged(Category = Category.ProxyKeepalive)]
        public Uri KeepAlive1 { get; set; }

        [Persistent("KeepAlive1Interval", DefaultValue = 120)]
        [CategoryNotifyPropertyChanged(Category = Category.ProxyKeepalive)]
        public int KeepAlive1Interval { get; set; }

        [NotifyPropertyChanged]
        public Uri KeepAlive1FullUri { get; set; }

        [Persistent("KeepAlive2")]
        [CategoryNotifyPropertyChanged(Category = Category.ProxyKeepalive)]
        public Uri KeepAlive2 { get; set; }

        [Persistent("KeepAlive2Interval", DefaultValue = 120)]
        [CategoryNotifyPropertyChanged(Category = Category.ProxyKeepalive)]
        public int KeepAlive2Interval { get; set; }

        [NotifyPropertyChanged]
        public Uri KeepAlive2FullUri { get; set; }

        [CategoryNotifyPropertyChanged(Category = Category.ProxyServer)]
        public int SocksListeningPort { get; set; }

        [Persistent("SocksListeningPort", DefaultValue = 1080)]
        public int PersistentSocksListeningPort { get; set; }

        private bool _canSetSocksListeningPort = true;
        [NotifyPropertyChanged]
        public bool CanSetSocksListeningPort
        {
            get { return _canSetSocksListeningPort; }
            set { _canSetSocksListeningPort = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the main window is visible or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if show; otherwise, <c>false</c>.
        /// </value>
        [NotifyPropertyChanged]
        public bool Show { get; set; }

        /// <summary>
        /// Gets or sets the connections count (for statistics).
        /// </summary>
        /// <value>
        /// The connections count.
        /// </value>
        [NotifyPropertyChanged]
        public int ConnectionsCount { get; set; }

        /// <summary>
        /// Gets or sets the bytes read (for statistics).
        /// </summary>
        /// <value>
        /// The bytes read.
        /// </value>
        [NotifyPropertyChanged]
        public long BytesRead { get; set; }

        /// <summary>
        /// Gets or sets the bytes written (for statistics).
        /// </summary>
        /// <value>
        /// The bytes written.
        /// </value>
        [NotifyPropertyChanged]
        public long BytesWritten { get; set; }

        private readonly object _statisticsLock = new object();

        /// <summary>
        /// Gets or sets the current view (right, this maybe shouldn't be here...).
        /// </summary>
        /// <value>
        /// The current view.
        /// </value>
        [Persistent("CurrentView", AutoSave = true)]
        [NotifyPropertyChanged]
        public Uri CurrentView { get; set; }

        /// <summary>
        /// Loads preferences and intializes proxy from them.
        /// </summary>
        public override void Load()
        {
            if (CurrentView == null)
                ShowAsyncLoad(FirstLoad);

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

        private void FirstLoad()
        {
            var diagnostic = SystemProxyAnalyzer.Diagnose();
            // if there is a local proxy, use it
            LocalProxy = diagnostic.DefaultProxy;
            // find a remote proxy
            if (!diagnostic.SensitiveHttpConnectRoute.HasFlag(RouteStatus.ProxyAcceptsAddress))
            {
                var firstProxy = ProxyPageScanner.ScanPage(ProxyPage.Default.First(), ProxyClient.CreateRoute(LocalProxy), TestTargetUri).FirstOrDefault();
                if (firstProxy != null)
                    RemoteProxy = new Uri(string.Format("http://{0}:{1}", firstProxy.Host, firstProxy.Port));
            }
            else
                RemoteProxy = null;
        }

        /// <summary>
        /// Called on connection to proxy server.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnProxyServerConnect(object sender, EventArgs e)
        {
            lock (_statisticsLock)
                ConnectionsCount++;
        }

        /// <summary>
        /// Called on transfer from proxy server (read or write).
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ProxyServerTransferEventArgs"/> instance containing the event data.</param>
        private void OnProxyServerTransfer(object sender, ProxyServerTransferEventArgs e)
        {
            lock (_statisticsLock)
            {
                BytesRead += e.BytesRead;
                BytesWritten += e.BytesWritten;
            }
        }

        /// <summary>
        /// Called when property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
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
            // this is a special case for debug (I have two instances of BlueDwarf running, so in Visual Studio, I force the port)
            if (CanSetSocksListeningPort)
                PersistentSocksListeningPort = SocksListeningPort;
            Persistence.Write();
        }

        /// <summary>
        /// Checks the proxy tunnel, and updates the configuration if it works.
        /// </summary>
        [Async(KillExisting = true)]
        private void CheckProxyTunnel()
        {
            var validTunnelSleep = TimeSpan.FromMinutes(5);
            var invalidTunnelSleep = TimeSpan.FromSeconds(10);
            for (; ; )
            {
                try
                {
                    SetStatusPending();
                    ProxyServer.Routes = null;
                    var routes = new List<Route>();
                    // iterative process, in order to have a quick working route
                    foreach (var route in GetRoutes())
                    {
                        routes.Add(route);
                        SetSuccessStatus(route);
                        ProxyServer.Routes = routes.ToArray();
                        RouteUpdated.Raise(this);
                    }
                    Thread.Sleep(validTunnelSleep);
                }
                catch (ProxyRouteException pre)
                {
                    SetFailureStatus(pre);
                    Thread.Sleep(invalidTunnelSleep);
                }
            }
        }

        private readonly object _keepAliveSerialLock = new object();
        private int _keepAliveSerial;

        /// <summary>
        /// Gets the available routes.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Route> GetRoutes()
        {
            var testTargetUri = TestTargetUri;
            // if remote is optional, whenever we have a remote proxy configured or not,
            // the short route (local only) is returned
            if (OptionalRemoteProxy)
            {
                var shortRoute = ProxyClient.CreateRoute(LocalProxy);
                if (testTargetUri != null)
                    ProxyValidator.Validate(shortRoute, testTargetUri);
                yield return shortRoute;
            }
            // then, if there is a remote proxy or it is not optional 
            // (in which case we skipped the first step above)
            // the full route is returned
            if (!OptionalRemoteProxy || RemoteProxy != null)
            {
                var fullRoute = ProxyClient.CreateRoute(LocalProxy, RemoteProxy);
                if (testTargetUri != null)
                    ProxyValidator.Validate(fullRoute, testTargetUri);
                yield return fullRoute;
            }
        }

        /// <summary>
        /// Keeps the connection alive by refreshing the web browser at specified interval.
        /// </summary>
        /// <param name="getBaseUri">The get base URI.</param>
        /// <param name="getInterval">The get interval.</param>
        /// <param name="setFullUri">The set full URI.</param>
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
        /// <summary>
        /// Setups the proxy server.
        /// </summary>
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

        /// <summary>
        /// Asks user to pick a remote proxy.
        /// </summary>
        public void PickRemoteProxy()
        {
            var viewModel = Navigator.Show<ProxyPickerViewModel>();
            if (viewModel != null && viewModel.ProxyServer != null)
                RemoteProxy = viewModel.ProxyServer.ProxyServer.Uri;
        }
    }
}
