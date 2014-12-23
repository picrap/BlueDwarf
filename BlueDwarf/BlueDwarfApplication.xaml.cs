using System;
using System.Windows;
using BlueDwarf.Navigation;
using BlueDwarf.Net.Proxy.Server;
using BlueDwarf.Utility;
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
        private class Options
        {
            [Option('p', "proxy-port", Required = true, HelpText = "Sockets proxy server port.")]
            public int ProxyPort { get; set; }

            [Option('m', "minimized", Required = true, HelpText = "Starts minimized.")]
            public bool Minimized { get; set; }
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
            StartupUtility.Register(GetType().Assembly, "-m");

            var container = new UnityContainer();
            UIConfiguration.Configure(container);
            CoreConfiguration.Configure(container);

            var proxyServer = container.Resolve<IProxyServer>();
            proxyServer.Start();

            var navigator = container.Resolve<INavigator>();
            navigator.Exiting += OnNavigatorExiting;
            var viewModel = navigator.Show(
                      delegate(ConfigurationViewModel vm)
                      {
                          if (options.Value.ProxyPort > 0)
                          {
                              vm.CanSetSocksListeningPort = false;
                              vm.SocksListeningPort = options.Value.ProxyPort;
                          }
                      });

            if (!options.Value.Minimized)
                viewModel.Show = true;
        }

        private void OnNavigatorExiting(object sender, EventArgs e)
        {
            StartupUtility.Unregister(GetType().Assembly);
        }
    }
}
