namespace ArxOne.MrAdvice.Utility
{
    using System;

    /// <summary>
    /// Extensions to types
    /// </summary>
    internal static class TypeExtensions
    {
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

    }
}