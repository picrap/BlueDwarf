using System;
using System.Windows;
using BlueDwarf.Utility;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;

namespace BlueDwarf.Aspects
{
    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Property, AllowMultiple = false, TargetMemberAttributes = MulticastAttributes.NonAbstract | MulticastAttributes.NonLiteral, PersistMetaData = true)]
    public class AutoDependencyProperty : LocationInterceptionAspect
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
        public AutoDependencyPropertyNotification Notification { get; set; }

        public override void RuntimeInitialize(LocationInfo locationInfo)
        {
            locationInfo.PropertyInfo.CreateDependencyProperty(DefaultValue, Notification);
        }

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            var dependencyProperty = args.Location.PropertyInfo.GetDependencyProperty();
            var dependencyObject = (DependencyObject)args.Instance;
            args.Value = dependencyObject.GetValue(dependencyProperty);
        }

        public override void OnSetValue(LocationInterceptionArgs args)
        {
            var dependencyProperty = args.Location.PropertyInfo.GetDependencyProperty();
            var dependencyObject = (DependencyObject)args.Instance;
            var oldValue = dependencyObject.GetValue(dependencyProperty);
            var newValue = args.Value;
            if (!oldValue.SafeEquals(newValue))
                dependencyObject.SetValue(dependencyProperty, newValue);
        }
    }
}
