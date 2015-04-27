// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using ArxOne.MrAdvice.MVVM.Navigation;
    using CommandLine;
    using Configuration;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using Net.Http;
    using Net.Name;
    using Net.Proxy;
    using Net.Proxy.Client;
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

        [Dependency]
        public INameResolver NameResolver { get; set; }

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
            SetupApplication();
            var proxyServer = ProxyServerFactory.CreateSocksProxyServer();
            ShowHome(Navigator, proxyServer, applicationOptions.ProxyPort, applicationOptions.Minimized, applicationOptions.FirstStart);
        }

        /// <summary>
        /// Setups the application.
        /// </summary>
        private void SetupApplication()
        {
            var thisAssembly = GetType().Assembly;
            SetupConfiguration.RegisterStartup(thisAssembly, "-m");
            if (SetupConfiguration.IsClickOnce)
                SetupConfiguration.SetUninstallIcon(thisAssembly);
        }

        private bool _updateChecked;
        private readonly Uri _applicationUri = new Uri("http://openstore.craponne.fr/BlueDwarf.exe");

        /// <summary>
        /// Checks if there is an update available.
        /// </summary>
        /// <param name="route">The route.</param>
        private void CheckUpdate(Route route)
        {
            if (SetupConfiguration.IsClickOnce)
                return;

            if (_updateChecked)
                return;

            try
            {
                using (var stream = route.Connect(_applicationUri, NameResolver))
                {
                    _updateChecked = true;

                    var request = HttpRequest.CreateGet(_applicationUri);
                    request.Verb = "HEAD";
                    request.Write(stream);
                    var response = HttpResponse.FromStream(stream);
                    var lastModified = response.Headers["last-modified"].FirstOrDefault();
                    DateTime lastModifiedDate;
                    if (lastModified == null || !DateTime.TryParse(lastModified, out lastModifiedDate))
                        return;

                    var thisAssemblyPath = GetType().Assembly.Location;

                    var oldVersions = Directory.GetFiles(Path.GetDirectoryName(thisAssemblyPath), Path.GetFileName(thisAssemblyPath) + ".old.*");
                    foreach (var oldVersion in oldVersions)
                    {
                        try
                        {
                            File.Delete(oldVersion);
                        }
                        catch (IOException)
                        { }
                    }

                    var thisAssemblyDate = File.GetLastWriteTime(thisAssemblyPath);
                    if (thisAssemblyDate < lastModifiedDate)
                        LoadUpdate(route);
                }
            }
            catch (ProxyRouteException)
            { }
        }

        /// <summary>
        /// Loads the update.
        /// </summary>
        /// <param name="route">The route.</param>
        private void LoadUpdate(Route route)
        {
            try
            {
                using (var stream = route.Connect(_applicationUri, NameResolver))
                {
                    HttpRequest.CreateGet(_applicationUri).Write(stream);
                    var response = HttpResponse.FromStream(stream);
                    var applicationBytes = response.ReadContent(stream);
                    var thisAssemblyPath = GetType().Assembly.Location;
                    var oldPath = thisAssemblyPath + ".old." + Guid.NewGuid();
                    File.Move(thisAssemblyPath, oldPath);
                    File.Delete(thisAssemblyPath);
                    File.WriteAllBytes(thisAssemblyPath, applicationBytes);
                    Process.Start(thisAssemblyPath, Environment.CommandLine);
                    Environment.Exit(0);
                }
            }
            catch (IOException)
            { }
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
        private void ShowHome(INavigator navigator, IProxyServer proxyServer, int socksListeningPort, bool minimized, bool firstStart)
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
                    vm.RouteUpdated += delegate { CheckUpdate(proxyServer.Routes.FirstOrDefault()); };
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
