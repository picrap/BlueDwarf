
namespace BlueDwarf.Configuration
{
    using System;
    using System.Collections.Generic;
    using Serialization;

    public class Persistence
    {
        public static readonly Persistence Current = new Persistence();

        private const string Key = "BlueDwarf";

        private readonly RegistrySerializer _serializer = new RegistrySerializer();

        private readonly IDictionary<string, object> _values = new Dictionary<string, object>();

        public object GetValue(string name, object defaultValue)
        {
            lock (_values)
            {
                object value;
                if (!_values.TryGetValue(name, out value))
                {
                    if (!_serializer.TryDeserialize(Key, name, out value))
                        value = defaultValue;
                    _values[name] = value;
                }
                return value;
            }
        }

        public object GetValue(string name, Type type, object defaultValue)
        {
            return ObjectReader.SafeConvert(GetValue(name, defaultValue), type);
        }

        public TValue GetValue<TValue>(string name, TValue defaultValue)
        {
            return (TValue)GetValue(name, typeof(TValue), defaultValue);
        }

        public void SetValue(string name, object value, bool writeNow)
        {
            _values[name] = value;
            if (writeNow)
                _serializer.Serialize(Key, name, value);
        }

        public void Write()
        {
            foreach (var kv in _values)
                _serializer.Serialize(Key, kv.Key, kv.Value);
        }
    }
}
