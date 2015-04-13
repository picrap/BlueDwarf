// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// Converts boolean to visibility
    /// </summary>
    public class VisibilityConverter : BoolConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = GetValue(value, parameter);
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
