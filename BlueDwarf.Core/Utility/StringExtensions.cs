// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Utility
{
    using System;

    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static Uri ToSafeUri(this string s)
        {
            try
            {
                if (s != null)
                    return new Uri(s);
            }
            catch (UriFormatException)
            { }
            return null;
        }
    }
}
