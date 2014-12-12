using System;
using System.Collections.Generic;

namespace BlueDwarf.Utility
{
    public static class TypeExtensions
    {
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static Type GetNullabled(this Type type)
        {
            if (!IsNullable(type))
                return null;
            return type.GetGenericArguments()[0];
        }

        public static IEnumerable<Type> GetSelfAndBaseTypes(this Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}
