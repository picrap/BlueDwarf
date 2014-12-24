using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using BlueDwarf.Utility;

namespace BlueDwarf.Aspects
{
    /// <summary>
    /// This class holds all auto DependencyProperties, grouped by control type
    /// </summary>
    public static class AutoDependencyProperties
    {
        private static readonly IDictionary<Type, IDictionary<string, DependencyProperty>> RegisteredTypes = new Dictionary<Type, IDictionary<string, DependencyProperty>>();

        /// <summary>
        /// Gets the dependency property matching the given PropertyInfo.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns></returns>
        public static DependencyProperty GetDependencyProperty(this PropertyInfo propertyInfo)
        {
            var dependencyProperties = GetDependencyProperties(propertyInfo);
            DependencyProperty property;
            dependencyProperties.TryGetValue(propertyInfo.Name, out property);
            return property;
        }

        /// <summary>
        /// Creates the dependency property related to a property.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="defaultValue">The default value (null if none).</param>
        /// <param name="notification">The notification type, in order to have a callback.</param>
        public static void CreateDependencyProperty(this PropertyInfo propertyInfo, object defaultValue, AutoDependencyPropertyNotification notification)
        {
            var dependencyProperties = GetDependencyProperties(propertyInfo);
            var ownerType = propertyInfo.DeclaringType;
            var propertyName = propertyInfo.Name;
            var defaultPropertyValue = defaultValue ?? propertyInfo.PropertyType.Default();
            var onPropertyChanged = GetPropertyChangedCallback(notification, propertyName, ownerType);
            if (propertyInfo.IsStatic())
            {
                // this does not work, and I'm not sure how to it, and even if we need it
            }
            else
            {
                dependencyProperties[propertyName] = DependencyProperty.Register(propertyName, propertyInfo.PropertyType, ownerType,
                    new PropertyMetadata(defaultPropertyValue, onPropertyChanged));
            }
        }

        /// <summary>
        /// Gets the property changed callback, based on notification type.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="ownerType">Type of the owner.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">notification</exception>
        private static PropertyChangedCallback GetPropertyChangedCallback(AutoDependencyPropertyNotification notification, string propertyName, Type ownerType)
        {
            switch (notification)
            {
                case AutoDependencyPropertyNotification.None:
                    return null;
                case AutoDependencyPropertyNotification.OnPropertyNameChanged:
                    return GetOnPropertyNameChangedCallback(propertyName, ownerType);
                default:
                    throw new ArgumentOutOfRangeException("notification");
            }
        }

        private static PropertyChangedCallback GetOnPropertyNameChangedCallback(string propertyName, Type ownerType)
        {
            PropertyChangedCallback onPropertyChanged;
            var methodName = string.Format("On{0}Changed", propertyName);
            var method = ownerType.GetMethod(methodName);
            if (method == null)
                throw new InvalidOperationException("Callback method not found (WTF?)");
            var parameters = method.GetParameters();
            if (parameters.Length == 0)
                onPropertyChanged = delegate(DependencyObject d, DependencyPropertyChangedEventArgs e) { method.Invoke(d, new object[0]); };
            else
                throw new InvalidOperationException("Unhandled method overload");
            return onPropertyChanged;
        }

        /// <summary>
        /// Gets the dependency properties group, based on property.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        /// <returns></returns>
        private static IDictionary<string, DependencyProperty> GetDependencyProperties(PropertyInfo propertyInfo)
        {
            IDictionary<string, DependencyProperty> dependencyProperties;
            var ownerType = propertyInfo.DeclaringType;
            if (!RegisteredTypes.TryGetValue(ownerType, out dependencyProperties))
                RegisteredTypes[ownerType] = dependencyProperties = new Dictionary<string, DependencyProperty>();
            return dependencyProperties;
        }
    }
}
