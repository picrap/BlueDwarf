// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf
{
    using Microsoft.Practices.Unity;
    using Navigation;
    using View;
    using ViewModel;

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
        /// Configures the specified container with application instances.
        /// </summary>
        /// <param name="container">The container.</param>
        public static void Configure(IUnityContainer container)
        {
            container.RegisterType<INavigator, Navigator>(AsSingleton());
            ConfigureViews(container);
        }

        private static void ConfigureViews(IUnityContainer container)
        {
            var navigator = container.Resolve<INavigator>();
            navigator.Configure<ConfigurationViewModel, ConfigurationView>();
            navigator.Configure<ProxyAnalysisViewModel, ProxyAnalysisView>();
            navigator.Configure<WebDownloaderViewModel, WebDownloaderView>();
            navigator.Configure<ProxyPickerViewModel, ProxyPickerView>();
        }
    }
}
