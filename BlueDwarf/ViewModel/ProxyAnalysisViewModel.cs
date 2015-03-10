// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.ViewModel
{
    using System;
    using Annotations;
    using ArxOne.MrAdvice.MVVM.Navigation;
    using ArxOne.MrAdvice.MVVM.Properties;
    using Microsoft.Practices.Unity;
    using Net.Proxy.Client.Diagnostic;
    using Properties;
    using Resources.Localization;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class ProxyAnalysisViewModel : ViewModel
    {
        [Dependency]
        public INavigator Navigator { get; set; }

        [Dependency]
        public ISystemProxyAnalyzer SystemProxyAnalyzer { get; set; }

        [NotifyPropertyChanged]
        public bool RequiresProxy { get; set; }

        [NotifyPropertyChanged]
        public bool DoesNotRequireProxy { get; set; }

        [NotifyPropertyChanged]
        public Uri DefaultProxy { get; set; }

        [NotifyPropertyChanged]
        public bool ProxyAllowsSensitiveSites { get; set; }

        [NotifyPropertyChanged]
        public bool DnsResolvesLocal { get; set; }

        [NotifyPropertyChanged]
        public bool DnsResolvesSensitiveSites { get; set; }

        [NotifyPropertyChanged]
        public bool ProxyConnectsToSensitiveIP { get; set; }

        [NotifyPropertyChanged]
        public bool WorkWithLocalProxy { get; set; }

        [NotifyPropertyChanged]
        public bool WorkWithTwoProxy { get; set; }

        [NotifyPropertyChanged]
        public bool WorkWithSomethingElse { get; set; }

        public override void Load()
        {
            ShowAsyncLoad(LoadDiagnostic);
        }

        /// <summary>
        /// Analyses proxy status, asynchronously.
        /// </summary>
        private void LoadDiagnostic()
        {
            var d = SystemProxyAnalyzer.Diagnose();
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
