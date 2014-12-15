
using System;
using System.Linq;
using System.Reflection;

namespace BlueDwarf.Utility
{
    public static class CustomAttributeProviderExtensions
    {
        /// <summary>
        /// Gets a single custom attribute, usable without this ugly cast.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="customAttributeProvider">The custom attribute provider.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this ICustomAttributeProvider customAttributeProvider, bool inherit = true)
            where TAttribute : Attribute
        {
            return customAttributeProvider.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().SingleOrDefault();
        }
    }
}
