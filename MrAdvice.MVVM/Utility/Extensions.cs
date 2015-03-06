
namespace ArxOne.MrAdvice.Utility
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Miscellaneous extensions (too lazy to sort them)
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Equals, with null support.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static bool SafeEquals(this object a, object b)
        {
            if (a == null || b == null)
                return (a == null) == (b == null);
            return a.Equals(b);
        }

        /// <summary>
        /// Creates a default instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object Default(this Type type)
        {
            if (type.IsClass)
                return null;
            return Activator.CreateInstance(type);
        }

        public static bool IsStatic(this PropertyInfo propertyInfo)
        {
            var getMethod = propertyInfo.GetGetMethod();
            if (getMethod != null)
                return getMethod.IsStatic;
            return propertyInfo.GetSetMethod().IsStatic;
        }
    }
}
