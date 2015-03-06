// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Controls
{
    using System.Windows.Controls;
    using DependencyProperty = ArxOne.MrAdvice.MVVM.Properties.DependencyProperty;

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
