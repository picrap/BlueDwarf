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

        [DataMember(Name = Preferences.LocalProxyKey)]
        [NotifyPropertyChanged]
        public virtual string LocalProxy { get; set; }

        [DataMember(Name = Preferences.RemoteProxyKey)]
        [NotifyPropertyChanged]
        public virtual string RemoteProxy { get; set; }

        [DataMember(Name = Preferences.TestTargetKey)]
        [NotifyPropertyChanged]
        public virtual string TestTarget { get; set; }

        [DataMember(Name = Preferences.KeepAlive1Key)]
        [NotifyPropertyChanged]
        public virtual string KeepAlive1 { get; set; }

        [DataMember(Name = Preferences.KeepAlive1IntervalKey)]
        [NotifyPropertyChanged]
        public virtual int KeepAlive1Interval { get; set; }

        [DataMember(Name = Preferences.KeepAlive2Key)]
        [NotifyPropertyChanged]
        public virtual string KeepAlive2 { get; set; }

        [DataMember(Name = Preferences.KeepAlive2IntervalKey)]
        [NotifyPropertyChanged]
        public virtual int KeepAlive2Interval { get; set; }

        [NotifyPropertyChanged]
        public virtual bool Hide { get; set; }

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
            PropertyChanged += OnPropertyChanged;
            CheckProxy();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePreferences();
            CheckProxy();
        }

        /// <summary>
        /// Updates the preferences.
        /// </summary>
        private void UpdatePreferences()
        {
            _serializer.Deserialize(_preferences, BlueDwarfKey);
            _objectReader.Map(this, _preferences);
            _serializer.Serialize(_preferences, BlueDwarfKey);
        }

        private Thread _proxyChecker;

        private void CheckProxy()
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

        public void AnalyzeProxy()
        {
            var analysis = Navigator.Show<ProxyAnalysisViewModel>();
            if (analysis != null)
                LocalProxy = analysis.DefaultProxy.ToString();
        }

        public void Minimize()
        {
            Hide = true;
        }

        public void Restore()
        {
            Hide = false;
        }

        public void Exit()
        {
            Navigator.Exit(true);
        }
    }
}
