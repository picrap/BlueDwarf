
namespace BlueDwarf.View
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Navigation;
    using ViewModel;

    /// <summary>
    /// Interaction logic for WebBrowserView.xaml
    /// </summary>
    public partial class WebDownloaderView
    {
        private dynamic ActiveXControl
        {
            get
            {
                // this is a brilliant way to access the WebBrowserObject prior to displaying the actual document (eg. Document property)
                FieldInfo fiComWebBrowser = typeof(System.Windows.Controls.WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
                if (fiComWebBrowser == null) return null;
                object objComWebBrowser = fiComWebBrowser.GetValue(Browser);
                return objComWebBrowser;
            }
        }

        public WebDownloaderView()
        {
            InitializeComponent();
        }

        private void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            ActiveXControl.Silent = true;
            var viewModel = DataContext as WebDownloaderViewModel;
            if (viewModel != null)
                Browser.Navigate(viewModel.DownloadUri);
        }

        private void OnLoadCompleted(object sender, NavigationEventArgs e)
        {
            var viewModel = DataContext as WebDownloaderViewModel;
            if (viewModel != null)
                viewModel.LoadCompleted(Browser.Document);
        }
    }
}
