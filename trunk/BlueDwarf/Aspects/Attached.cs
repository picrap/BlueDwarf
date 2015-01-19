// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/
namespace BlueDwarf.Aspects
{
    using System;
    using PostSharp.Aspects;
    using PostSharp.Aspects.Configuration;
    using PostSharp.Extensibility;
    using PostSharp.Reflection;

    /// <summary>
    /// Aspect for attached properties
    /// See Property for more information
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [MulticastAttributeUsage(MulticastTargets.Property, PersistMetaData = true)]
    [Serializable]
    [LocationInterceptionAspectConfiguration(AspectPriority = 100)]
    public class Attached : Aspect, ILocationInterceptionAspect
    {
        /// <summary>
        /// Gets or sets the default value for the dependency property.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the notification type.
        /// </summary>
        /// <value>
        /// The notification.
        /// </value>
        public DependencyPropertyNotification Notification { get; set; }

        /// <summary>
        /// The CurrentProperty is used by Property
        /// The syntax for using all of this is elegant, much more elegant than the implementation
        /// </summary>
        [ThreadStatic]
        internal static System.Windows.DependencyProperty CurrentProperty;

        private static readonly object[] NoParameter = new object[0];

        public void RuntimeInitialize(LocationInfo locationInfo)
        {
            var propertyInfo = locationInfo.PropertyInfo;
            propertyInfo.CreateDependencyProperty(DefaultValue, Notification);
            if (propertyInfo.GetValue(null, NoParameter) == null)
                propertyInfo.SetValue(null, Activator.CreateInstance(propertyInfo.PropertyType), NoParameter);
        }

        public void OnGetValue(LocationInterceptionArgs args)
        {
            SetCurrent(args);
            args.ProceedGetValue();
        }

        public void OnSetValue(LocationInterceptionArgs args)
        {
            SetCurrent(args);
            args.ProceedSetValue();
        }

        private static void SetCurrent(LocationInterceptionArgs args)
        {
            CurrentProperty = args.Location.PropertyInfo.GetDependencyProperty();
        }
    }
}