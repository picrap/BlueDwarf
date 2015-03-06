// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using ArxOne.MrAdvice.Utility;
    using FirstFloor.ModernUI.Windows.Controls;
    using Utility;

    /// <summary>
    /// Extensions to ModernUI
    /// </summary>
    public static class ModernUI
    {
        public static readonly DependencyProperty DialogButtonsProperty = DependencyProperty.RegisterAttached(
            "DialogButtons", typeof(Button[]), typeof(ModernUI), new PropertyMetadata(default(object), OnSetDialogButtons));

        public static void SetDialogButtons(DependencyObject element, object value)
        {
            element.SetValue(DialogButtonsProperty, value);
        }

        public static object GetDialogButtons(DependencyObject element)
        {
            return element.GetValue(DialogButtonsProperty);
        }

        private static void OnSetDialogButtons(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var modernDialog = d.GetLogicalSelfAndParents().OfType<ModernDialog>().FirstOrDefault();
            if (modernDialog != null)
                modernDialog.Buttons = (IEnumerable<Button>)e.NewValue;
        }
    }
}
