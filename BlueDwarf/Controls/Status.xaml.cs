// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Controls
{
    using System.Windows;
    using ArxOne.MrAdvice.MVVM.Properties;
    using DependencyProperty = ArxOne.MrAdvice.MVVM.Properties.DependencyProperty;

    /// <summary>
    /// Status class displays status (as expected)
    /// </summary>
    public partial class Status
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DependencyProperty(Notification = DependencyPropertyNotification.OnPropertyNameChanged)]
        public StatusCode Code { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Status"/> class.
        /// </summary>
        public Status()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when [code changed].
        /// </summary>
        public void OnCodeChanged()
        {
            OK.Visibility = Code == StatusCode.OK ? Visibility.Visible : Visibility.Collapsed;
            Error.Visibility = Code == StatusCode.Error ? Visibility.Visible : Visibility.Collapsed;
            Pending.Visibility = Code == StatusCode.Pending ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
