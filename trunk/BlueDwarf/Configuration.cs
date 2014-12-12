using BlueDwarf.Net.Name;
using BlueDwarf.Net.Proxy.Client;
using BlueDwarf.Net.Proxy.Server;
using BlueDwarf.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace BlueDwarf
{
    public static class Configuration
    {
        private static LifetimeManager AsSingleton()
        {
            return new ContainerControlledLifetimeManager();
        }

        public static void ConfigureApplication(IUnityContainer container)
        {
            container.AddNewExtension<Interception>();
            container.RegisterType<IProxyClient, TunnelProxyClient>(AsSingleton());
            container.RegisterType<IProxyServer, SocksProxyServer>(AsSingleton());
            container.RegisterType<INameResolver, StatDnsNameResolver>(AsSingleton());
            container.RegisterType<ConfigurationViewModel, ConfigurationViewModel>(AsSingleton(), new Interceptor<VirtualMethodInterceptor>(), new InterceptionBehavior<NotifyPropertyChangedBehavior>());
        }
    }
}
