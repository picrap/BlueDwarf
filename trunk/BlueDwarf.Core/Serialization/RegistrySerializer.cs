// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Serialization
{
    using System;
    using System.Linq;
    using Microsoft.Win32;
    using Utility;

    /// <summary>
    /// Serializes data from and to registry
    /// </summary>
    public class RegistrySerializer
    {
        private readonly ObjectReader _reader = new ObjectReader();

        private static RegistryKey CreateSubKey(string node)
        {
            return Registry.CurrentUser.CreateSubKey(@"Software\" + node);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <param name="node">The registry node (under HKCU/Sofware).</param>
        public void Serialize(object o, string node)
        {
            var data = _reader.Read(o);
            using (var r = CreateSubKey(node))
            {
                foreach (var kv in data)
                {
                    var v = GetValue(kv.Value);
                    r.SetValue(kv.Key, v.Item1 ?? new byte[0], v.Item2);
                }
            }
        }

        /// <summary>
        /// Deserializes the specified object.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <param name="node">The node.</param>
        public void Deserialize(object o, string node)
        {
            using (var r = CreateSubKey(node))
            {
                var data = r.GetValueNames().ToDictionary(n => n, n => ReadValue(r, n));
                _reader.Write(o, data);
            }
        }

        /// <summary>
        /// Serializes the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void Serialize(string node, string name, object value)
        {
            using (var r = CreateSubKey(node))
            {
                var qualifiedValue = GetValue(value);
                r.SetValue(name, qualifiedValue.Item1 ?? new byte[0], qualifiedValue.Item2);
            }
        }

        /// <summary>
        /// Tries the deserialize.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryDeserialize(string node, string name, out object value)
        {
            using (var r = CreateSubKey(node))
            {
                if (!r.GetValueNames().Contains(name))
                {
                    value = null;
                    return false;
                }
                value = ReadValue(r, name);
                return true;
            }
        }

        private static object ReadValue(RegistryKey r, string n)
        {
            if (r.GetValueKind(n) == RegistryValueKind.None)
                return null;
            return r.GetValue(n);
        }

        private static Tuple<object, RegistryValueKind> GetValue(object o)
        {
            if (o == null)
                return Tuple.Create<object, RegistryValueKind>(null, RegistryValueKind.None);
            var t = o.GetType();
            return GetValue(o, t);
        }

        private static Tuple<object, RegistryValueKind> GetValue(object o, Type t)
        {
            if (t.IsNullable())
                return GetValue(o, t.GetNullabled());

            if (t == typeof(string))
                return Tuple.Create(o, RegistryValueKind.String);
            if (t == typeof(Uri))
                return Tuple.Create(o, RegistryValueKind.String);

            if (t == typeof(int))
                return Tuple.Create(o, RegistryValueKind.DWord);

            if (t == typeof(long))
                return Tuple.Create(o, RegistryValueKind.QWord);

            throw new ArgumentException(@"Unsupported type", "t");
        }
    }
}
