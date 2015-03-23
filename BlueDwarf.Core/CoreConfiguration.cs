// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf
{
    using Configuration;
    using Microsoft.Practices.Unity;
    using Net;
    using Net.Geolocation;
    using Net.Geolocation.Telize;
    using Net.Name;
    using Net.Proxy;
    using Net.Proxy.Client;
    using Net.Proxy.Client.Diagnostic;
    using Net.Proxy.Scanner;
    using Net.Proxy.Server;

    public static class CoreConfiguration
    {
        /// <summary>
        /// Specifies instance as singleton (laziness follows).
        /// </summary>
        /// <returns></returns>
        private static LifetimeManager AsSingleton()
        {
            return new ContainerControlledLifetimeManager();
        }

        /// <summary>
        /// Configures the specified container for core.
        /// </summary>
        /// <param name="container">The container.</param>
        public static void Configure(IUnityContainer container)
        {
            container.RegisterType<IProxyClient, TunnelProxyClient>(AsSingleton());
            container.RegisterType<IProxyServerFactory, ProxyServerFactory>(AsSingleton());
            container.RegisterType<INameResolver, MultiNameResolver>(AsSingleton());
            container.RegisterType<ISystemProxyAnalyzer, SystemProxyAnalyzer>(AsSingleton());
            container.RegisterType<IPersistence, RegistryPersistence>(AsSingleton());
            container.RegisterType<ISetupConfiguration, SetupConfiguration>(AsSingleton());
            container.RegisterType<IProxyConfiguration, ProxyConfiguration>(AsSingleton());
            container.RegisterType<IDownloader, Downloader>(AsSingleton());
            container.RegisterType<IHostScanner, HostScanner>(AsSingleton());
            container.RegisterType<IProxyValidator, ProxyValidator>(AsSingleton());
            container.RegisterType<IProxyPageScanner, ProxyPageScanner>(AsSingleton());
            container.RegisterType<IProxyAnalyzer, ProxyAnalyzer>(AsSingleton());
            container.RegisterType<IGeolocation, TelizeGeolocation>(AsSingleton());
        }
    }
}