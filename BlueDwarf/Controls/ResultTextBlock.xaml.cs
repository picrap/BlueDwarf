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
        [AutoDependencyProperty]
        public string Text { get; set; }

        [AutoDependencyProperty]
        public bool Success { get; set; }

        public ResultTextBlock()
        {
            InitializeComponent();
        }
    }
}
