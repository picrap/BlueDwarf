// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Aspects
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Configuration;

    public class PropertyImporter<TInterface>
    {
        private readonly PropertyInfo _propertyInfo;

        public PropertyImporter(Type instanceType)
        {
            var propertyInfo = instanceType.GetProperties().SingleOrDefault(p => typeof(TInterface).IsAssignableFrom(p.PropertyType));
            if (propertyInfo == null)
                throw new NotImplementedException(string.Format("The type {0} must have a property of type {1}", instanceType.Name, typeof(IPersistence).Name));
            _propertyInfo = propertyInfo;
        }

        public TInterface Get(object target)
        {
            return (TInterface)_propertyInfo.GetValue(target, new object[0]);
        }
    }
}
