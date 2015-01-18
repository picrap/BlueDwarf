
namespace BlueDwarf.Controls
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Navigation;
    using Aspects;

    public partial class ExtendedWebBrowser
    {
        [Aspects.DependencyProperty(Notification = DependencyPropertyNotification.OnPropertyNameChanged)]
        public Uri Uri { get; set; }

        [Aspects.DependencyProperty]
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

        private bool? _silent;

        public bool Silent
        {
            get { return ActiveXControl.Silent; }
            set
            {
                var activeXControl = ActiveXControl;
                if (activeXControl != null)
                    activeXControl.Silent = value;
                else
                    _silent = value;
            }
        }

        public ExtendedWebBrowser()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_silent.HasValue)
                ActiveXControl.Silent = _silent.Value;
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
