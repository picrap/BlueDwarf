// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Controls
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Converts (almost) anything to boolean
    /// </summary>
    public class BoolConverter: IValueConverter
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
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetValue(value, parameter);
        }

        /// <summary>
        /// Gets the value as boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        protected static bool GetValue(object value, object parameter)
        {
            var boolValue = GetValue(value);
            var stringParameter = parameter as string;
            if (stringParameter == "not")
                boolValue = !boolValue;
            return boolValue;
        }

        /// <summary>
        /// Gets the value as boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static bool GetValue(object value)
        {
            if (value is bool)
                return (bool)value;
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
