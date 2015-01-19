// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Configuration
{
    using System.Collections.Generic;
    using Annotations;
    using Serialization;

    /// <summary>
    /// Implementation with registry of IPersistence
    /// </summary>
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class RegistryPersistence : IPersistence
    {
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
