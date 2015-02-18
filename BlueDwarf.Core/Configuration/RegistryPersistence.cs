// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Configuration
{
    using System.Collections.Generic;
    using Annotations;
    using Serialization;

    /// <summary>
    /// Implementation with registry of IPersistence
    /// </summary>
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    internal class RegistryPersistence : IPersistence
    {
        private const string Key = "BlueDwarf";

        private readonly RegistrySerializer _serializer = new RegistrySerializer();

        private readonly IDictionary<string, object> _values = new Dictionary<string, object>();

        /// <summary>
        /// Gets a value, or default value.
        /// Searches in cache first, then loads from registry
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="writeNow">if set to <c>true</c> persists the value immediately.</param>
        public void SetValue(string name, object value, bool writeNow)
        {
            _values[name] = value;
            if (writeNow)
                _serializer.Serialize(Key, name, value);
        }

        /// <summary>
        /// Writes all changes down to registry.
        /// </summary>
        public void Write()
        {
            foreach (var kv in _values)
                _serializer.Serialize(Key, kv.Key, kv.Value);
        }
    }
}
