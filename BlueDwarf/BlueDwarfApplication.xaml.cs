// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf
{
    using System;
    using System.Windows;
    using CommandLine;
    using Microsoft.Practices.Unity;
    using Navigation;
    using Net.Proxy.Server;
    using Utility;
    using ViewModel;

    /// <summary>
    /// Application
    /// </summary>
    public partial class BlueDwarfApplication
    {
        private class Options
        {
            [Option('p', "proxy-port", Required = false, HelpText = "Sockets proxy server port.")]
            public int ProxyPort { get; set; }

            [Option('m', "minimized", Required = false, HelpText = "Starts minimized.")]
            public bool Minimized { get; set; }

            [Option('d', "download", Required = false, HelpText = "Downloads the URI.")]
            public string DownloadUri { get; set; }

            [Option('t', "save-text", Required = false, HelpText = "Saves the file as text.")]
            public string SaveTextPath { get; set; }
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
            // Unity container configuration
            var container = new UnityContainer();
            UIConfiguration.Configure(container);
            CoreConfiguration.Configure(container);

            // Navigator
            var navigator = container.Resolve<INavigator>();
            navigator.Exiting += OnNavigatorExiting;

            // Command-line options
            var options = Parser.Default.ParseArguments<Options>(Environment.GetCommandLineArgs());

            // Special request for download
            if (options.Value.DownloadUri != null)
                DownloadFile(navigator, options.Value.DownloadUri, options.Value.SaveTextPath);
            else // Main behavior: configuration window
            {
                StartupUtility.Register(GetType().Assembly, "-m");
                container.Resolve<IProxyServer>().Start();
                ShowConfiguration(navigator, options.Value.ProxyPort, options.Value.Minimized);
            }
        }

        private static void DownloadFile(INavigator navigator, string downloadUri, string saveTextPath)
        {
            navigator.Show(
                delegate(WebDownloaderViewModel vm)
                {
                    vm.DownloadUri = downloadUri;
                    vm.SaveTextPath = saveTextPath;
                });
        }

        private static void ShowConfiguration(INavigator navigator, int socksListeningPort, bool minimized)
        {
            var viewModel = navigator.Show(
                delegate(ConfigurationViewModel vm)
                {
                    if (socksListeningPort > 0)
                    {
                        vm.CanSetSocksListeningPort = false;
                        vm.SocksListeningPort = socksListeningPort;
                    }
                });

            if (!minimized)
                viewModel.Show = true;
        }

        private void OnNavigatorExiting(object sender, EventArgs e)
        {
            StartupUtility.Unregister(GetType().Assembly);
        }
    }
}
