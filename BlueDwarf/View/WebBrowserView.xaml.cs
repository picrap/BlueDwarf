using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using BlueDwarf.Utility;

namespace BlueDwarf.View
{
    /// <summary>
    /// Specific code to integrate a WebBrowser.
    /// Unfortunately the WebBrowser class can not be inherited
    /// </summary>
    public partial class WebBrowserView
    {
        #region public dependency string Source { get; set; }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(string), typeof(WebBrowserView), new PropertyMetadata(default(string), OnSourceChanged));

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rthis = (WebBrowserView)d;
            rthis.Refresh();
        }

        #endregion

        #region public dependency bool AppendRandomQueryParameterProperty { get; set; }

        public static readonly DependencyProperty AppendRandomQueryParameterProperty = DependencyProperty.Register(
            "AppendRandomQueryParameter", typeof(bool), typeof(WebBrowserView), new PropertyMetadata(default(bool), OnAppendRandomQueryParameterChanged));

        public bool AppendRandomQueryParameter
        {
            get { return (bool)GetValue(AppendRandomQueryParameterProperty); }
            set { SetValue(AppendRandomQueryParameterProperty, value); }
        }

        private static void OnAppendRandomQueryParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rthis = (WebBrowserView)d;
            rthis.Refresh();
        }

        #endregion

        #region public dependency int WebBrowserView { get; set; }

        public static readonly DependencyProperty RefreshIntervalProperty = DependencyProperty.Register(
            "RefreshInterval", typeof(int), typeof(WebBrowserView), new PropertyMetadata(default(int)));

        public int RefreshInterval
        {
            get { return (int)GetValue(RefreshIntervalProperty); }
            set { SetValue(RefreshIntervalProperty, value); }
        }

        #endregion

        public WebBrowserView()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private Thread _refreshThread;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _refreshThread = ThreadHelper.Create(BackgroundRefresh);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _refreshThread.Abort();
        }

        /// <summary>
        /// Background thread worker.
        /// Refreshes the browser content.
        /// Note this has to be done in UI thread (hence the Dispatcher.Invoke)
        /// </summary>
        private void BackgroundRefresh()
        {
            for (; ; )
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
