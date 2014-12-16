using System;

namespace BlueDwarf.Utility
{
    /// <summary>
    /// Type converter, with more conversion than System.Convert
    /// </summary>
    public static class ObjectTypeConverter
    {
        /// <summary>
        /// Converts the specified object to target type.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidCastException">Can not create URI</exception>
        public static object Convert(object o, Type targetType)
        {
            if (o == null)
                return CreateDefault(targetType);

            if (targetType == typeof(Uri))
            {
                try
                {
                    return new Uri(Convert<string>(o));
                }
                catch (UriFormatException e)
                {
                    throw new InvalidCastException("Can not create URI", e);
                }
            }

            var sourceType = o.GetType();
            if (sourceType == typeof(Uri))
                return Convert(o.ToString(), targetType);

            var c = System.Convert.ChangeType(o, targetType);
            return c;
        }

        /// <summary>
        /// Converts the specified object to target type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static T Convert<T>(object o)
        {
            return (T)Convert(o, typeof(T));
        }

        public static object CreateDefault(Type targetType)
        {
            if (targetType.IsClass)
                return null;
            return Activator.CreateInstance(targetType);
        }
    }
}
