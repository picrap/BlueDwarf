using BlueDwarf.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace BlueDwarf
{
    public static class UIConfiguration
    {
        private static LifetimeManager AsSingleton()
        {
            return new ContainerControlledLifetimeManager();
        }

        public static void Configure(IUnityContainer container)
        {
            container.AddNewExtension<Interception>();
            container.RegisterType<ConfigurationViewModel, ConfigurationViewModel>(AsSingleton(), new Interceptor<VirtualMethodInterceptor>(), new InterceptionBehavior<NotifyPropertyChangedBehavior>());
        }
    }
}
