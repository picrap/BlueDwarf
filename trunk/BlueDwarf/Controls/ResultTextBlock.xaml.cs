using System.Windows.Controls;
using BlueDwarf.Aspects;

namespace BlueDwarf.Controls
{
    /// <summary>
    /// ResultTextBlock.xaml
    /// Dude, this is unused :(
    /// </summary>
    public partial class ResultTextBlock : UserControl
    {
        [DependencyProperty]
        public string Text { get; set; }

        [DependencyProperty]
        public bool Success { get; set; }

        public ResultTextBlock()
        {
            InitializeComponent();
        }
    }
}
