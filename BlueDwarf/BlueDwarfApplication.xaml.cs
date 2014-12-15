
using System.Windows;
using BlueDwarf.Core;
using BlueDwarf.Net.Proxy.Server;
using BlueDwarf.View;
using BlueDwarf.ViewModel;
using Microsoft.Practices.Unity;

namespace BlueDwarf
{
    /// <summary>
    ///     Logique d'interaction pour BlueDwarfApplication.xaml
    /// </summary>
    public partial class BlueDwarfApplication
    {
        public BlueDwarfApplication()
        {
            Startup += OnStartup;
        }

        private static void OnStartup(object sender, StartupEventArgs e)
        {
            var container = new UnityContainer();
            UIConfiguration.Configure(container);
            CoreConfiguration.Configure(container);

            var proxyServer = container.Resolve<IProxyServer>();
            proxyServer.Start();

            var view = container.Resolve<ConfigurationView>();
            var viewModel = container.Resolve<ConfigurationViewModel>();
            viewModel.Load();
            view.DataContext = viewModel;
            view.Show();
        }
    }
}
