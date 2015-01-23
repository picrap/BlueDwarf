// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf
{
    using Configuration;
    using Microsoft.Practices.Unity;
    using Net;
    using Net.Name;
    using Net.Proxy;
    using Net.Proxy.Client;
    using Net.Proxy.Client.Diagnostic;
    using Net.Proxy.Scanner;
    using Net.Proxy.Server;

    public static class CoreConfiguration
    {
        private static LifetimeManager AsSingleton()
        {
            return new ContainerControlledLifetimeManager();
        }

        public static void Configure(IUnityContainer container)
        {
            container.RegisterType<IProxyClient, TunnelProxyClient>(AsSingleton());
            container.RegisterType<IProxyServerFactory, ProxyServerFactory>(AsSingleton());
            container.RegisterType<INameResolver, StatDnsNameResolver>(AsSingleton());
            container.RegisterType<IProxyAnalyzer, ProxyAnalyzer>(AsSingleton());
            container.RegisterType<IPersistence, RegistryPersistence>(AsSingleton());
            container.RegisterType<IStartupConfiguration, MenuStartupConfiguration>(AsSingleton());
            container.RegisterType<IProxyConfiguration, ProxyConfiguration>(AsSingleton());
            container.RegisterType<IDownloader, Downloader>(AsSingleton());
            container.RegisterType<IHostScanner, HostScanner>(AsSingleton());
        }
    }
}