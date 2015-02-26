// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Utility
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Data;
    using ViewModel.Properties;

    internal static class BindingReader
    {
        class DummyDO : DependencyObject
        {
            public object Value
            {
                get { return (object)GetValue(ValueProperty); }
                set { SetValue(ValueProperty, value); }
            }

            public static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register("Value", typeof(object), typeof(DummyDO), new UIPropertyMetadata(null));

        }

        public static object EvalBinding(Binding b)
        {
            DummyDO d = new DummyDO();
            BindingOperations.SetBinding(d, DummyDO.ValueProperty, b);
            return d.Value;
        }
    }

    internal class PropertyBinder : DependencyObject
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(PropertyBinder), new UIPropertyMetadata(null, OnValueChanged));

        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var binder = (PropertyBinder)d;
            binder._propertyInfo.SetValue(binder._target, binder.Value, new object[0]);
        }

        private readonly object _target;
        private readonly PropertyInfo _propertyInfo;

        private PropertyBinder(object target, PropertyInfo propertyInfo, Binding binding)
        {
            _target = target;
            _propertyInfo = propertyInfo;
            //binding.Mode=BindingMode.TwoWay;
            binding.NotifyOnSourceUpdated = true;
        Binding.SourceUpdatedEvent.AddOwner(GetType());
            BindingOperations.SetBinding(this, ValueProperty, binding);
        }

        public static object Bind(object target, PropertyInfo propertyInfo, Binding binding)
        {
            return new PropertyBinder(target, propertyInfo, binding).Value;
        }
    }
}
