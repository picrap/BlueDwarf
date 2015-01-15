using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using BlueDwarf.Aspects;
using BlueDwarf.Utility;

namespace BlueDwarf.Controls
{
    /// <summary>
    /// Specific code to integrate a WebBrowser.
    /// Unfortunately the WebBrowser class can not be inherited
    /// </summary>
    public partial class RefreshWebBrowser
    {
        [AutoDependencyProperty(Notification = AutoDependencyPropertyNotification.OnPropertyNameChanged)]
        public string Source { get; set; }

        [AutoDependencyProperty(Notification = AutoDependencyPropertyNotification.OnPropertyNameChanged)]
        public bool AppendRandomQueryParameter { get; set; }

        [AutoDependencyProperty]
        public int RefreshInterval { get; set; }

        public RefreshWebBrowser()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        public void OnSourceChanged()
        {
            Refresh();
        }

        public void OnAppendRandomQueryParameterChanged()
        {
            Refresh();
        }

        private Thread _refreshThread;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _runBackgroundRefresh = true;
            BackgroundRefresh();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _runBackgroundRefresh = false;
        }

        private bool _runBackgroundRefresh;

        /// <summary>
        /// Background thread worker.
        /// Refreshes the browser content.
        /// Note this has to be done in UI thread (hence the Dispatcher.Invoke)
        /// </summary>
        [Async(ThreadName = "BackgroundRefresh")]
        private void BackgroundRefresh()
        {
            while (_runBackgroundRefresh)
            {
                var interval = Dispatcher.Invoke(() => RefreshInterval);
                if (interval == 0)
                {
                    Thread.Sleep(10000);
                    return;
                }

                Thread.Sleep(TimeSpan.FromSeconds(interval));
                Dispatcher.Invoke(Refresh);
            }
        }

        /// <summary>
        /// Refreshes the browser content.
        /// </summary>
        private void Refresh()
        {
            var uri = GetUri();
            if (uri != null)
                WebBrowser.Navigate(uri);
        }

        private int _randomValue = 0;

        /// <summary>
        /// Gets the URI, optionnally appended with a random value.
        /// </summary>
        /// <returns></returns>
        private Uri GetUri()
        {
            if (Source == null)
                return null;
            try
            {
                var rawUri = new Uri(Source);
                if (!AppendRandomQueryParameter)
                    return rawUri;

                var randomParameter = string.Format("whatthefook={0}", ++_randomValue);
                if (rawUri.Query.IsNullOrEmpty())
                    return new Uri(Source + "?" + randomParameter);
                return new Uri(Source + "&" + randomParameter);
            }
            catch (UriFormatException)
            {
            }
            return null;
        }
    }
}
