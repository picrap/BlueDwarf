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
            var diagnostic = SystemProxyAnalyzer.Diagnose();
            RequiresProxy = diagnostic.DefaultProxy != null;
            DoesNotRequireProxy = !RequiresProxy;
            DefaultProxy = diagnostic.DefaultProxy;
            ProxyAllowsSensitiveSites = diagnostic.SensitiveHttpGetRoute.HasFlag(RouteStatus.ProxyAcceptsName)
                                        && diagnostic.SensitiveHttpsConnectRoute.HasFlag(RouteStatus.ProxyAcceptsName)
                                        && diagnostic.SensitiveHttpConnectRoute.HasFlag(RouteStatus.ProxyAcceptsName);
            DnsResolvesLocal = diagnostic.SafeLocalDns;
            DnsResolvesSensitiveSites = diagnostic.SensitiveLocalDns;
            ProxyConnectsToSensitiveIP = diagnostic.SensitiveHttpsConnectRoute.HasFlag(RouteStatus.ProxyAcceptsAddress)
                                         && diagnostic.SensitiveHttpConnectRoute.HasFlag(RouteStatus.ProxyAcceptsAddress);

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
