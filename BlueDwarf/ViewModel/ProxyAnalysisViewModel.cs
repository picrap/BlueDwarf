
using System;
using BlueDwarf.Annotations;
using BlueDwarf.Aspects;
using BlueDwarf.Navigation;
using BlueDwarf.Net.Proxy.Client.Diagnostic;
using Microsoft.Practices.Unity;

namespace BlueDwarf.ViewModel
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class ProxyAnalysisViewModel : ViewModel
    {
        [Dependency]
        public INavigator Navigator { get; set; }

        [Dependency]
        public IProxyAnalyzer ProxyAnalyzer { get; set; }

        [AutoNotifyPropertyChanged]
        public bool RequiresProxy { get; set; }

        [AutoNotifyPropertyChanged]
        public bool DoesNotRequireProxy { get; set; }

        [AutoNotifyPropertyChanged]
        public Uri DefaultProxy { get; set; }

        [AutoNotifyPropertyChanged]
        public bool ProxyAllowsSensitiveSites { get; set; }

        [AutoNotifyPropertyChanged]
        public bool DnsResolvesLocal { get; set; }

        [AutoNotifyPropertyChanged]
        public bool DnsResolvesSensitiveSites { get; set; }

        [AutoNotifyPropertyChanged]
        public bool ProxyConnectsToSensitiveIP { get; set; }

        [AutoNotifyPropertyChanged]
        public bool WorkWithLocalProxy { get; set; }

        [AutoNotifyPropertyChanged]
        public bool WorkWithTwoProxy { get; set; }

        [AutoNotifyPropertyChanged]
        public bool WorkWithSomethingElse { get; set; }

        public override void Load()
        {
            Async(LoadAsync);
        }

        /// <summary>
        /// Analyses proxy status, asynchronously.
        /// </summary>
        private void LoadAsync()
        {
            var d = ProxyAnalyzer.Diagnose();
            RequiresProxy = d.DefaultProxy != null;
            DoesNotRequireProxy = !RequiresProxy;
            DefaultProxy = d.DefaultProxy;
            ProxyAllowsSensitiveSites = d.SensitiveHttpGetRoute.HasFlag(RouteStatus.ProxyAcceptsName)
                                        && d.SensitiveHttpsConnectRoute.HasFlag(RouteStatus.ProxyAcceptsName)
                                        && d.SensitiveHttpConnectRoute.HasFlag(RouteStatus.ProxyAcceptsName);
            DnsResolvesLocal = d.SafeLocalDns;
            DnsResolvesSensitiveSites = d.SensitiveLocalDns;
            ProxyConnectsToSensitiveIP = d.SensitiveHttpsConnectRoute.HasFlag(RouteStatus.ProxyAcceptsAddress)
                                         && d.SensitiveHttpConnectRoute.HasFlag(RouteStatus.ProxyAcceptsAddress);

            if (DnsResolvesSensitiveSites && ProxyConnectsToSensitiveIP)
                WorkWithLocalProxy = true;
            else if (!ProxyConnectsToSensitiveIP)
                WorkWithTwoProxy = true;
            else
                WorkWithSomethingElse = true;
        }

        /// <summary>
        /// Asks to apply recommandations
        /// </summary>
        public void Apply()
        {
            Navigator.Exit(true);
        }

        /// <summary>
        /// Asks not to apply th recommandations.
        /// </summary>
        public void Cancel()
        {
            Navigator.Exit(false);
        }
    }
}
