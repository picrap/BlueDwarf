// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Controls
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using Resources.Localization;
    using Utility;

    public class UnitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var format = parameter as string;
            if (format == "B")
                return ConvertUnit(value, CommonLocale.B, 1024, "", CommonLocale.ki, CommonLocale.Mi, CommonLocale.Gi);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static string ConvertUnit(object value, string unit, int prefixUnit, params string[] prefixes)
        {
            var v = ObjectTypeConverter.Convert<decimal>(value);
            int prefixIndex = 0;
            for (; prefixIndex < prefixes.Length - 1; prefixIndex++)
            {
                if (v < prefixUnit)
                    break;
                v /= prefixUnit;
            }
            var cultureInfo = CultureInfo.GetCultureInfo(CommonLocale.Language);
            var literal = string.Format(cultureInfo, "{0:#,#0.##} {1}{2}", Math.Round(v, 2), prefixes[prefixIndex], unit);
            return literal;
        }
    }
}
