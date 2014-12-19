using System;
using System.Windows;
using System.Windows.Threading;
using BlueDwarf.Navigation;
using BlueDwarf.Net.Proxy.Server;
using BlueDwarf.ViewModel;
using CommandLine;
using Microsoft.Practices.Unity;

namespace BlueDwarf
{
    /// <summary>
    /// Application
    /// </summary>
    public partial class BlueDwarfApplication
    {
        public class Options
        {
            [Option('p', "proxy-port", Required = true, HelpText = "Sockets proxy server port.")]
            public int ProxyPort { get; set; }
        }

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
            var options = Parser.Default.ParseArguments<Options>(Environment.GetCommandLineArgs());

            var container = new UnityContainer();
            UIConfiguration.Configure(container);
            CoreConfiguration.Configure(container);

            var proxyServer = container.Resolve<IProxyServer>();
            proxyServer.Start();

            var navigator = container.Resolve<INavigator>();
            var viewModel = navigator.Show<ConfigurationViewModel>(
                      delegate(ConfigurationViewModel vm)
                      {
                          if (options.Value.ProxyPort > 0)
                          {
                              vm.CanSetSocksListeningPort = false;
                              vm.SocksListeningPort = options.Value.ProxyPort;
                          }
                      });
#if DEBUG
            //viewModel.Show = true;
#endif
        }
    }
}
