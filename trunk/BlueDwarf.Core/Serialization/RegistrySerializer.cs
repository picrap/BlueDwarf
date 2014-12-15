using System;
using System.Linq;
using BlueDwarf.Utility;
using Microsoft.Win32;

namespace BlueDwarf.Serialization
{
    public class RegistrySerializer
    {
        private readonly ObjectReader _reader = new ObjectReader();

        public void Serialize(object o, string node)
        {
            var data = _reader.Read(o);
            using (var r = Registry.CurrentUser.CreateSubKey(@"Software\" + node))
            {
                foreach (var kv in data)
                {
                    var v = GetValue(kv.Value);
                    r.SetValue(kv.Key, v.Item1 ?? new byte[0], v.Item2);
                }
            }
        }

        public void Deserialize(object o, string node)
        {
            using (var r = Registry.CurrentUser.CreateSubKey(@"Software\" + node))
            {
                var data = r.GetValueNames().ToDictionary(n => n, n => ReadValue(r, n));
                _reader.Write(o, data);
            }
        }

        private static object ReadValue(RegistryKey r, string n)
        {
            if (r.GetValueKind(n) == RegistryValueKind.None)
                return null;
            return r.GetValue(n);
        }

        private Tuple<object, RegistryValueKind> GetValue(object o)
        {
            if (o == null)
                return Tuple.Create<object, RegistryValueKind>(null, RegistryValueKind.None);
            var t = o.GetType();
            return GetValue(o, t);
        }

        private Tuple<object, RegistryValueKind> GetValue(object o, Type t)
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
