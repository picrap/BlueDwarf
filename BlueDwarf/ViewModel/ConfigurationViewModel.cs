using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading;
using BlueDwarf.Annotations;
using BlueDwarf.Navigation;
using BlueDwarf.Net.Proxy.Client;
using BlueDwarf.Net.Proxy.Server;
using BlueDwarf.Serialization;
using BlueDwarf.Utility;
using Microsoft.Practices.Unity;

namespace BlueDwarf.ViewModel
{
    /// <summary>
    /// Configuration view-model.
    /// This is the main view
    /// </summary>
    [DataContract]
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class ConfigurationViewModel : ViewModel
    {
        [Dependency]
        public IProxyClient ProxyClient { get; set; }

        [Dependency]
        public IProxyServer ProxyServer { get; set; }

        [Dependency]
        public INavigator Navigator { get; set; }

        private const string BlueDwarfKey = "BlueDwarf";

        public enum Category
        {
            None = 0,
            ProxyTunnel,
            ProxyServer,
            ProxyKeepalive,
        }

        [DataMember(Name = Preferences.LocalProxyKey)]
        [NotifyPropertyChanged(Category = Category.ProxyTunnel)]
        public virtual Uri LocalProxy { get; set; }

        [DataMember(Name = Preferences.RemoteProxyKey)]
        [NotifyPropertyChanged(Category = Category.ProxyTunnel)]
        public virtual Uri RemoteProxy { get; set; }

        [DataMember(Name = Preferences.TestTargetKey)]
        [NotifyPropertyChanged(Category = Category.ProxyTunnel)]
        public virtual string TestTarget { get; set; }

        [DataMember(Name = Preferences.KeepAlive1Key)]
        [NotifyPropertyChanged(Category = Category.ProxyKeepalive)]
        public virtual Uri KeepAlive1 { get; set; }

        [DataMember(Name = Preferences.KeepAlive1IntervalKey)]
        [NotifyPropertyChanged(Category = Category.ProxyKeepalive)]
        public virtual int KeepAlive1Interval { get; set; }

        [DataMember(Name = Preferences.KeepAlive2Key)]
        [NotifyPropertyChanged(Category = Category.ProxyKeepalive)]
        public virtual Uri KeepAlive2 { get; set; }

        [DataMember(Name = Preferences.KeepAlive2IntervalKey)]
        [NotifyPropertyChanged(Category = Category.ProxyKeepalive)]
        public virtual int KeepAlive2Interval { get; set; }

        [NotifyPropertyChanged(Category = Category.ProxyServer)]
        public virtual int SocksListeningPort { get; set; }

        private bool _canSetSocksListeningPort = true;
        [NotifyPropertyChanged]
        public virtual bool CanSetSocksListeningPort
        {
            get { return _canSetSocksListeningPort; }
            set { _canSetSocksListeningPort = value; }
        }

        [NotifyPropertyChanged]
        public virtual bool Show { get; set; }

        private readonly RegistrySerializer _serializer = new RegistrySerializer();

        private readonly Preferences _preferences = new Preferences();
        private readonly ObjectReader _objectReader = new ObjectReader();

        /// <summary>
        /// Loads preferences and intializes proxy from them.
        /// </summary>
        public override void Load()
        {
            _serializer.Deserialize(_preferences, BlueDwarfKey);
            _objectReader.Map(_preferences, this);
            if (CanSetSocksListeningPort)
                SocksListeningPort = _preferences.SocksListeningPort;
            PropertyChanged += OnPropertyChanged;
            CheckProxyTunnel();
            SetupProxyServer();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePreferences();
            switch (this.GetCategory<Category>(e.PropertyName))
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
            _serializer.Deserialize(_preferences, BlueDwarfKey);
            _objectReader.Map(this, _preferences);
            if (CanSetSocksListeningPort)
                _preferences.SocksListeningPort = SocksListeningPort;
            _serializer.Serialize(_preferences, BlueDwarfKey);
        }

        private Thread _proxyChecker;

        private void CheckProxyTunnel()
        {
            var proxyChecker = _proxyChecker;
            if (proxyChecker != null)
                _proxyChecker.Abort();

            _proxyChecker = ThreadHelper.CreateBackground(
                delegate
                {
                    var route = ProxyClient.CreateRoute(_preferences.TestTarget, _preferences.LocalProxy, _preferences.RemoteProxy);
                    if (route != null)
                        ProxyServer.ProxyRoute = route;
                    _proxyChecker = null;
                });
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
