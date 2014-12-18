using System.Windows;
using System.Windows.Controls;

namespace BlueDwarf.Controls
{
    /// <summary>
    /// Logique d'interaction pour ResultTextBlock.xaml
    /// </summary>
    public partial class ResultTextBlock : UserControl
    {
        #region dependecy public string Text { get; set; }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof (string), typeof (ResultTextBlock), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion

        #region dependecy public bool Success { get; set; }

        public static readonly DependencyProperty SuccessProperty = DependencyProperty.Register(
            "Success", typeof (bool), typeof (ResultTextBlock), new PropertyMetadata(default(bool)));

        public bool Success
        {
            get { return (bool) GetValue(SuccessProperty); }
            set { SetValue(SuccessProperty, value); }
        }

        #endregion

        public ResultTextBlock()
        {
            InitializeComponent();
        }
    }
}
