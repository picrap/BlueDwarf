
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using BlueDwarf.Utility;

namespace BlueDwarf.Serialization
{
    public class ObjectReader
    {
        private static readonly object[] NoParameter = new object[0];

        public IDictionary<string, object> Read(object o)
        {
            var properties = GetSerializableProperties(o);
            return properties.ToDictionary(p => p.Item1, p => p.Item2.GetValue(o, NoParameter));
        }

        public void Write(object o, IDictionary<string, object> values)
        {
            var properties = GetSerializableProperties(o);
            foreach (var property in properties)
            {
                object value;
                if (values.TryGetValue(property.Item1, out value))
                    property.Item2.SetValue(o, SafeConvert(value, property.Item2.PropertyType), NoParameter);
            }
        }

        private static object SafeConvert(object value, Type targetType)
        {
            try
            {
                return ObjectTypeConverter.Convert(value, targetType);
            }
            catch (InvalidCastException)
            { }
            return ObjectTypeConverter.CreateDefault(targetType);
        }

        public void Map(object a, object b)
        {
            var data = Read(a);
            Write(b, data);
        }

        private IEnumerable<Tuple<string, PropertyInfo>> GetSerializableProperties(object o)
        {
            var allProperties = o.GetType().GetProperties()
                .Where(p => p.GetIndexParameters().Length == 0).ToArray(); // all public properties without index

            // Support serializable (partially... We could use ISerializable too... But this is too far out of scope)
            var serializableProperties = allProperties.Where(p => p.GetCustomAttribute<SerializableAttribute>() != null)
                .Select(p => Tuple.Create(p.Name, p)).ToArray();
            if (serializableProperties.Length > 0)
                return serializableProperties;

            // Also support DataContract
            if (o.GetType().GetSelfAndBaseTypes().Any(t => t.GetCustomAttribute<DataContractAttribute>() != null))
            {
                var dataMemberProperties = from p in allProperties
                                           let a = p.GetCustomAttribute<DataMemberAttribute>()
                                           where a != null
                                           select Tuple.Create(a.Name ?? p.Name, p);
                return dataMemberProperties;
            }

            // In the end, get them all
            return allProperties.Select(p => Tuple.Create(p.Name, p)).ToArray();

        }
    }
}
