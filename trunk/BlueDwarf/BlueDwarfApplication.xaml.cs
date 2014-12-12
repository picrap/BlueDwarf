
using System.Windows;
using BlueDwarf.Net.Proxy;
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

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var container = new UnityContainer();
            Configuration.ConfigureApplication(container);

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
