// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Utility
{
    using System;

    /// <summary>
    /// Extensions to <see cref="string"/>
    /// </summary>
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// Converts to URI, or null if it is no valid URI.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
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
