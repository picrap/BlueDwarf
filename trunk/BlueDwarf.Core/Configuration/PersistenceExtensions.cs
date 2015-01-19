
namespace BlueDwarf.Configuration
{
    using System;
    using Serialization;

    public static class PersistenceExtensions
    {
        public static object GetValue(this IPersistence persistence, string name, Type type, object defaultValue)
        {
            return ObjectReader.SafeConvert(persistence.GetValue(name, defaultValue), type);
        }

        public static TValue GetValue<TValue>(this IPersistence persistence, string name, TValue defaultValue)
        {
            return (TValue)persistence.GetValue(name, typeof(TValue), defaultValue);
        }

    }
}
