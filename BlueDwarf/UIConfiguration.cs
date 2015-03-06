// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf
{
    using System.Windows;
    using ArxOne.MrAdvice.MVVM.Navigation;
    using ArxOne.MrAdvice.Utility;
    using Microsoft.Practices.Unity;

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
            container.RegisterInstance(typeof(INavigator), Application.Current.GetNavigator());
        }
    }
}
