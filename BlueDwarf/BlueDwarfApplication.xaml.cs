// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf
{
    using System;
    using System.Windows;
    using ArxOne.MrAdvice.MVVM.Navigation;
    using CommandLine;
    using Configuration;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
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
        public ISetupConfiguration SetupConfiguration { get; set; }

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

        /// <summary>
        /// Starts the main application.
        /// </summary>
        /// <param name="applicationOptions">The application options.</param>
        private void StartMain(ApplicationOptions applicationOptions)
        {
            var thisAssembly = GetType().Assembly;
            SetupConfiguration.RegisterStartup(thisAssembly, "-m");
            SetupConfiguration.SetUninstallIcon(thisAssembly);
            var proxyServer = ProxyServerFactory.CreateSocksProxyServer();
            ShowHome(Navigator, proxyServer, applicationOptions.ProxyPort, applicationOptions.Minimized, applicationOptions.FirstStart);
        }

        /// <summary>
        /// Configures the unity container.
        /// </summary>
        private void ConfigureContainer()
        {
            var container = new UnityContainer();
            // Dude, I just realized that this is not automatic (Why?)
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
            UIConfiguration.Configure(container);
            CoreConfiguration.Configure(container);
            container.BuildUp(this);
        }

        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="navigator">The navigator.</param>
        /// <param name="downloadUri">The download URI.</param>
        /// <param name="saveTextPath">The save text path.</param>
        /// <param name="proxy">The proxy.</param>
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

        /// <summary>
        /// Shows the home view.
        /// </summary>
        /// <param name="navigator">The navigator.</param>
        /// <param name="proxyServer">The proxy server.</param>
        /// <param name="socksListeningPort">The socks listening port.</param>
        /// <param name="minimized">if set to <c>true</c> [minimized].</param>
        /// <param name="firstStart">if set to <c>true</c> [first start].</param>
        private static void ShowHome(INavigator navigator, IProxyServer proxyServer, int socksListeningPort, bool minimized, bool firstStart)
        {
            var viewModel = navigator.Show(
                delegate(HomeViewModel vm)
                {
                    if (firstStart)
                        vm.CurrentView = null;
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

        /// <summary>
        /// Called on <see cref="INavigator.Exiting"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnNavigatorExiting(object sender, EventArgs e)
        {
            SetupConfiguration.UnregisterStartup(GetType().Assembly);
        }
    }
}
