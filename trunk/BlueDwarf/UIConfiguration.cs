using BlueDwarf.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace BlueDwarf
{
    public static class UIConfiguration
    {
        /// <summary>
        /// Just a nicer way to use singletons
        /// </summary>
        /// <returns></returns>
        private static LifetimeManager AsSingleton()
        {
            return new ContainerControlledLifetimeManager();
        }

        /// <summary>
        /// Creates view-model aspects.
        /// </summary>
        /// <returns></returns>
        private static InjectionMember[] AsViewModel()
        {
            return new InjectionMember[] { new Interceptor<VirtualMethodInterceptor>(), new InterceptionBehavior<NotifyPropertyChangedBehavior>() };
        }

        /// <summary>
        /// Configures the specified container with application instances.
        /// </summary>
        /// <param name="container">The container.</param>
        public static void Configure(IUnityContainer container)
        {
            container.AddNewExtension<Interception>();
            container.RegisterType<ConfigurationViewModel, ConfigurationViewModel>(AsSingleton(), AsViewModel());
        }
    }
}
