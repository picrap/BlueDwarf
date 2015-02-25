// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Utility
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public static class InstanceData
    {
        private static readonly IList<Tuple<WeakReference, IDictionary<string, object>>> Data = new List<Tuple<WeakReference, IDictionary<string, object>>>();

        private static void Cleanup()
        {
            lock (Data)
            {
                for (int index = 0; index < Data.Count; )
                {
                    if (Data[index].Item1.IsAlive)
                        index++;
                    else
                        Data.RemoveAt(index);
                }
            }
        }

        private static IDictionary<string, object> GetDataDictionary(object instance)
        {
            lock (Data)
            {
                Cleanup();
                var dataDictionary = Data.FirstOrDefault(d => d.Item1.Target == instance);
                if (dataDictionary == null)
                {
                    dataDictionary = new Tuple<WeakReference, IDictionary<string, object>>(new WeakReference(instance), new ConcurrentDictionary<string, object>());
                    Data.Add(dataDictionary);
                }
                return dataDictionary.Item2;
            }
        }

        public static bool TryGetData(object instance, string key, out object value)
        {
            return GetDataDictionary(instance).TryGetValue(key, out value);
        }

        public static void SetData(object instance, string key, object value)
        {
            GetDataDictionary(instance)[key] = value;
        }

        public static TValue GetData<TValue>(object instance, string key)
        {
            object value;
            if (TryGetData(instance, key, out value))
                return (TValue)value;
            return default(TValue);
        }
    }
}
