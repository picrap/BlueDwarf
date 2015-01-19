
using System.Windows;
using BlueDwarf.Aspects;
using DependencyProperty = BlueDwarf.Aspects.DependencyProperty;

namespace BlueDwarf.Controls
{
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
