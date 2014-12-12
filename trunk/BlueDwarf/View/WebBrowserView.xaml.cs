using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using BlueDwarf.Utility;

namespace BlueDwarf.View
{
    /// <summary>
    /// Logique d'interaction pour WebBrowserView.xaml
    /// </summary>
    public partial class WebBrowserView
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(string), typeof(WebBrowserView), new PropertyMetadata(default(string), OnSourceChanged));

        public new string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rthis = (WebBrowserView)d;
            rthis.Refresh();
        }

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

        public static readonly DependencyProperty RefreshIntervalProperty = DependencyProperty.Register(
            "RefreshInterval", typeof(int), typeof(WebBrowserView), new PropertyMetadata(default(int)));

        public int RefreshInterval
        {
            get { return (int)GetValue(RefreshIntervalProperty); }
            set { SetValue(RefreshIntervalProperty, value); }
        }

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
            _refreshThread = new Thread(BackgroundRefresh) { IsBackground = true };
            _refreshThread.Start();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _refreshThread.Abort();
        }

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

        private void Refresh()
        {
            var uri = GetUri();
            if (uri != null)
                WebBrowser.Navigate(uri);
        }

        private int _randomValue = 0;

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
