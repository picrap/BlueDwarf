
namespace BlueDwarf.Aspects
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Configuration;
    using PostSharp.Aspects;
    using PostSharp.Reflection;

    public class PropertyImporter<TInterface>
    {
        private readonly PropertyInfo _propertyInfo;

        public PropertyImporter(LocationInfo locationInfo)
        {
            var instanceType = locationInfo.PropertyInfo.DeclaringType;
            var propertyInfo = instanceType.GetProperties().SingleOrDefault(p => typeof(TInterface).IsAssignableFrom(p.PropertyType));
            if (propertyInfo == null)
                throw new NotImplementedException(string.Format("The type {0} must have a property of type {1}", instanceType.Name, typeof(IPersistence).Name));
            _propertyInfo = propertyInfo;
        }

        public TInterface Get(LocationInterceptionArgs args)
        {
            return (TInterface)_propertyInfo.GetValue(args.Instance, new object[0]);
        }
    }
}
