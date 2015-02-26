// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf
{
    using System;
    using System.Windows;
    using CommandLine;
    using Configuration;
    using Microsoft.Practices.Unity;
    using Navigation;
    using Net.Proxy;
    using Net.Proxy.Server;
    using Utility;
    using ViewModel;

    /// <summary>
    /// Application
    /// </summary>
    public partial class BlueDwarfApplication
    {
        [Dependency]
        public IStartupConfiguration StartupConfiguration { get; set; }

        [Dependency]
        public INavigator Navigator { get; set; }

        [Dependency]
        public IProxyConfiguration ProxyConfiguration { get; set; }

        [Dependency]
        public IProxyServerFactory ProxyServerFactory { get; set; }

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
            ConfigureContainer();
            Navigator.Exiting += OnNavigatorExiting;

            // Command-line options
            var options = Parser.Default.ParseArguments<ApplicationOptions>(Environment.GetCommandLineArgs());
            var applicationOptions = options.Value;

            // Special request for download
            if (applicationOptions.DownloadUri != null)
                DownloadFile(Navigator, applicationOptions.DownloadUri, applicationOptions.SaveTextPath, applicationOptions.Proxy.ToSafeUri());
            else // Main behavior: configuration window
                StartMain(applicationOptions);
        }

        private void StartMain(ApplicationOptions applicationOptions)
        {
            StartupConfiguration.Register(GetType().Assembly, "-m");
            var proxyServer = ProxyServerFactory.CreateSocksProxyServer();
            ShowConfiguration(Navigator, proxyServer, applicationOptions.ProxyPort, applicationOptions.Minimized);
        }

        private void ConfigureContainer()
        {
            var container = new UnityContainer();
            UIConfiguration.Configure(container);
            CoreConfiguration.Configure(container);
            container.BuildUp(this);
        }

        private void DownloadFile(INavigator navigator, string downloadUri, string saveTextPath, Uri proxy)
        {
            if (proxy != null)
                ProxyConfiguration.SetApplicationProxy(proxy);
            navigator.Show(
                delegate(WebDownloaderViewModel vm)
                {
                    vm.DownloadUri = downloadUri;
                    vm.SaveTextPath = saveTextPath;
                });
        }

        private static void ShowConfiguration(INavigator navigator, IProxyServer proxyServer, int socksListeningPort, bool minimized)
        {
            var viewModel = navigator.Show(
                delegate(HomeViewModel vm)
                {
                    vm.ProxyServer = proxyServer;
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
            StartupConfiguration.Unregister(GetType().Assembly);
        }
    }
}
