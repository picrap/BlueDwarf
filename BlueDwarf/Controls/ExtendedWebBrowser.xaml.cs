// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Controls
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using Aspects;

    /// <summary>
    /// Extensions to web browser. Unfortunately the WebBrowser class can not be overriden (WTF?)
    /// </summary>
    public partial class ExtendedWebBrowser
    {
        [Aspects.DependencyProperty(Notification = DependencyPropertyNotification.OnPropertyNameChanged)]
        public Uri Uri { get; set; }

        [Aspects.DependencyProperty]
        public string Text { get; set; }

        /// <summary>
        /// Gets the document (as dynamic, since dynamic compensates my laziness).
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        private dynamic Document
        {
            get { return WebBrowser.Document; }
        }

        private object _activeXControl;

        /// <summary>
        /// Gets the inner ActiveX control.
        /// </summary>
        /// <value>
        /// The active x control.
        /// </value>
        private dynamic ActiveXControl
        {
            get
            {
                if (_activeXControl == null)
                {
                    // this is a brilliant way to access the WebBrowserObject prior to displaying the actual document (eg. Document property)
                    var webBrowserField = typeof(WebBrowser).GetField("_axIWebBrowser2",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                    if (webBrowserField == null)
                        return null;
                    _activeXControl = webBrowserField.GetValue(WebBrowser);
                }
                return _activeXControl;
            }
        }

        private bool? _silent;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExtendedWebBrowser"/> is silent.
        /// If Silent, no alert box is shown
        /// </summary>
        /// <value>
        ///   <c>true</c> if silent; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Called when Uri changed.
        /// (raised automatically)
        /// </summary>
        public void OnUriChanged()
        {
            WebBrowser.Navigate(Uri);
        }
    }
}
