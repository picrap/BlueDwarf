﻿using System;
using System.Windows;
using BlueDwarf.Navigation;
using BlueDwarf.Net.Proxy.Server;
using BlueDwarf.Utility;
using BlueDwarf.ViewModel;
using CommandLine;
using Microsoft.Practices.Unity;

namespace BlueDwarf
{
    using System.Threading;
    using System.Windows.Controls;
    using View;

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

            StartupUtility.Register(GetType().Assembly, "-m");

            // Command-line options
            var options = Parser.Default.ParseArguments<Options>(Environment.GetCommandLineArgs());

            // Special request for download
            if (options.Value.DownloadUri != null)
            {
                navigator.Show(
                          delegate(WebDownloaderViewModel vm)
                          {
                              if (options.Value.ProxyPort > 0)
                              {
                                  vm.DownloadUri = options.Value.DownloadUri;
                                  vm.SaveTextPath = options.Value.SaveTextPath;
                              }
                          });
                return;
            }

            var proxyServer = container.Resolve<IProxyServer>();
            proxyServer.Start();

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

        private void DownloadUri(string downloadUri, string saveTextPath)
        {
            var webBrowser = new WebDownloaderView();
            webBrowser.Show();
            var signal = new EventWaitHandle(false, EventResetMode.ManualReset);
            webBrowser.Browser.LoadCompleted += delegate { signal.Set(); };
            webBrowser.Browser.Navigate(downloadUri);
            signal.WaitOne(TimeSpan.FromSeconds(10));
            dynamic document = webBrowser.Browser.Document;
            var text = document.DomDocument.selection.createRange().text;
        }
    }
}
