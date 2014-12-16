
using System.Windows;
using BlueDwarf.Net.Proxy.Server;
using BlueDwarf.View;
using BlueDwarf.ViewModel;
using Microsoft.Practices.Unity;

namespace BlueDwarf
{
    /// <summary>
    /// Application
    /// </summary>
    public partial class BlueDwarfApplication: INavigator
    {
        private ConfigurationView _view;

        /// <summary>
        /// Application startup code.
        /// Creates and configures container
        /// Instantiates view and view-models
        /// And rocks!
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StartupEventArgs"/> instance containing the event data.</param>
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var container = new UnityContainer();
            UIConfiguration.Configure(container);
            CoreConfiguration.Configure(container);
            container.RegisterInstance(typeof (INavigator), this);

            var proxyServer = container.Resolve<IProxyServer>();
            proxyServer.Start();

            _view = container.Resolve<ConfigurationView>();
            var viewModel = container.Resolve<ConfigurationViewModel>();
            viewModel.Load();
            _view.DataContext = viewModel;
            _view.Show();
        }

        void INavigator.Exit()
        {
            Shutdown();
        }
    }
}
