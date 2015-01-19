// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Controls
{
    using System.Windows.Controls;
    using View;
    using View.Properties;

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
