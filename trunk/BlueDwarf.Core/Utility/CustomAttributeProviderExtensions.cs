
using System;
using System.Linq;
using System.Reflection;

namespace BlueDwarf.Utility
{
    public static class CustomAttributeProviderExtensions
    {
        public static TAttribute GetCustomAttribute<TAttribute>(this ICustomAttributeProvider customAttributeProvider, bool inherit = true)
            where TAttribute : Attribute
        {
            return customAttributeProvider.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().SingleOrDefault();
        }
    }
}
