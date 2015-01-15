
using System.Windows;
using BlueDwarf.Aspects;

namespace BlueDwarf.Controls
{
    public partial class Status
    {
        [AutoDependencyProperty(Notification = AutoDependencyPropertyNotification.OnPropertyNameChanged)]
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
