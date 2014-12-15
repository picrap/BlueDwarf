using System;

namespace BlueDwarf.Utility
{
    public static class ObjectTypeConverter
    {
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
