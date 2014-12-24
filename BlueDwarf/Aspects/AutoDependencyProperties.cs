using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using BlueDwarf.Utility;

namespace BlueDwarf.Aspects
{
    public static class AutoDependencyProperties
    {
        private static readonly IDictionary<Type, IDictionary<string, DependencyProperty>> RegisteredTypes = new Dictionary<Type, IDictionary<string, DependencyProperty>>();

        public static DependencyProperty GetDependencyProperty(this PropertyInfo propertyInfo)
        {
            var dependencyProperties = GetDependencyProperties(propertyInfo);
            DependencyProperty property;
            dependencyProperties.TryGetValue(propertyInfo.Name, out property);
            return property;
        }

        public static void CreateDependencyProperty(this PropertyInfo propertyInfo, object defaultValue, AutoDependencyPropertyNotification notification)
        {
            var dependencyProperties = GetDependencyProperties(propertyInfo);
            var ownerType = propertyInfo.ReflectedType;
            var propertyName = propertyInfo.Name;
            var defaultPropertyValue = defaultValue ?? propertyInfo.PropertyType.Default();
            var onPropertyChanged = GetPropertyChangedCallback(notification, propertyName, ownerType);
            if (propertyInfo.IsStatic())
            {
                dependencyProperties[propertyName] = DependencyProperty.RegisterAttached(propertyName, propertyInfo.PropertyType, ownerType,
                    new PropertyMetadata(defaultPropertyValue, onPropertyChanged));
            }
            else
            {
                dependencyProperties[propertyName] = DependencyProperty.Register(propertyName, propertyInfo.PropertyType, ownerType,
                    new PropertyMetadata(defaultPropertyValue, onPropertyChanged));
            }
        }

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

        private static IDictionary<string, DependencyProperty> GetDependencyProperties(PropertyInfo property)
        {
            IDictionary<string, DependencyProperty> dependencyProperties;
            var ownerType = property.ReflectedType;
            if (!RegisteredTypes.TryGetValue(ownerType, out dependencyProperties))
                RegisteredTypes[ownerType] = dependencyProperties = new Dictionary<string, DependencyProperty>();
            return dependencyProperties;
        }
    }
}
