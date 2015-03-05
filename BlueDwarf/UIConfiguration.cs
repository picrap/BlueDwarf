// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf
{
    using Microsoft.Practices.Unity;
    using Navigation;

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
        }
    }
}
