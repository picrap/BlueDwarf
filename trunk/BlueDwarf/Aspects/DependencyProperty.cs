using System;
using System.Windows;
using BlueDwarf.Utility;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;

namespace BlueDwarf.Aspects
{
    using PostSharp.Aspects.Configuration;

    /// <summary>
    /// Marks a simple auto property to be bound to a DependencyProperty
    /// This attribute is handled by Postsharp, and by some unexplainable magic, works.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [MulticastAttributeUsage(MulticastTargets.Property, PersistMetaData = true)]
    [Serializable]
    [LocationInterceptionAspectConfiguration(AspectPriority = 100)]
    public class DependencyProperty : Aspect, ILocationInterceptionAspect
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
        /// Initializes the current aspect.
        /// </summary>
        /// <param name="locationInfo">Location to which the current aspect is applied.</param>
        public void RuntimeInitialize(LocationInfo locationInfo)
        {
            locationInfo.PropertyInfo.CreateDependencyProperty(DefaultValue, Notification);
        }

        /// <summary>
        /// Method invoked <i>instead</i> of the <c>Get</c> semantic of the field or property to which the current aspect is applied,
        /// i.e. when the value of this field or property is retrieved.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public void OnGetValue(LocationInterceptionArgs args)
        {
            var dependencyProperty = args.Location.PropertyInfo.GetDependencyProperty();
            var dependencyObject = (DependencyObject)args.Instance;
            // yes, in the end, it is a GetValue()
            args.Value = dependencyObject.GetValue(dependencyProperty);
        }

        /// <summary>
        /// Method invoked <i>instead</i> of the <c>Set</c> semantic of the field or property to which the current aspect is applied,
        /// i.e. when the value of this field or property is changed.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public void OnSetValue(LocationInterceptionArgs args)
        {
            var dependencyProperty = args.Location.PropertyInfo.GetDependencyProperty();
            var dependencyObject = (DependencyObject)args.Instance;
            var oldValue = dependencyObject.GetValue(dependencyProperty);
            var newValue = args.Value;
            // not sure it is necessary to check for a change
            if (!oldValue.SafeEquals(newValue))
                dependencyObject.SetValue(dependencyProperty, newValue);
        }
    }
}
