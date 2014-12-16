using BlueDwarf.Net.Name;
using BlueDwarf.Net.Proxy.Client;
using BlueDwarf.Net.Proxy.Server;
using Microsoft.Practices.Unity;

namespace BlueDwarf
{
    public class CoreConfiguration
    {
        private static LifetimeManager AsSingleton()
        {
            return new ContainerControlledLifetimeManager();
        }

        public static void Configure(IUnityContainer container)
        {
            container.RegisterType<IProxyClient, TunnelProxyClient>(AsSingleton());
            container.RegisterType<IProxyServer, SocksProxyServer>(AsSingleton());
            container.RegisterType<INameResolver, StatDnsNameResolver>(AsSingleton());
        }
    }
}