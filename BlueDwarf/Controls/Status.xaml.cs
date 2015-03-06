// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Controls
{
    using System.Windows;
    using ArxOne.MrAdvice.MVVM.Properties;
    using DependencyProperty = ArxOne.MrAdvice.MVVM.Properties.DependencyProperty;

    public partial class Status
    {
        [DependencyProperty(Notification = DependencyPropertyNotification.OnPropertyNameChanged)]
        public StatusCode Code { get; set; }

        public Status()
        {
            InitializeComponent();
        }

        public void OnCodeChanged()
        {
            OK.Visibility = Code == StatusCode.OK ? Visibility.Visible : Visibility.Collapsed;
            Error.Visibility = Code == StatusCode.Error ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
