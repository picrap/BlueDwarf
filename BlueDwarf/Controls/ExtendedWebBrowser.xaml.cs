
namespace BlueDwarf.Controls
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Navigation;
    using Aspects;

    public partial class ExtendedWebBrowser
    {
        [AutoDependencyProperty(Notification = AutoDependencyPropertyNotification.OnPropertyNameChanged)]
        public Uri Uri { get; set; }

        [AutoDependencyProperty]
        public string Text { get; set; }

        private dynamic Document
        {
            get { return WebBrowser.Document; }
        }

        private object _activeXControl;

        private dynamic ActiveXControl
        {
            get
            {
                if (_activeXControl == null)
                {
                    // this is a brilliant way to access the WebBrowserObject prior to displaying the actual document (eg. Document property)
                    var webBrowserField = typeof(System.Windows.Controls.WebBrowser).GetField("_axIWebBrowser2",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                    if (webBrowserField == null)
                        return null;
                    _activeXControl = webBrowserField.GetValue(WebBrowser);
                }
                return _activeXControl;
            }
        }

        public bool Silent
        {
            get { return ActiveXControl.Silent; }
            set { ActiveXControl.Silent = value; }
        }

        public ExtendedWebBrowser()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ActiveXControl.Silent = true;
            WebBrowser.LoadCompleted += OnLoadCompleted;
            if (Uri != null)
                WebBrowser.Navigate(Uri);
        }

        private void OnLoadCompleted(object sender, NavigationEventArgs e)
        {
            Text = Document.body.innerText;
        }

        public void OnUriChanged()
        {
            WebBrowser.Navigate(Uri);
        }
    }
}
