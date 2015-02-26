// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Controls
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Xaml;
    using Utility;

    public class BindAnyExtension : MarkupExtension
    {
        private readonly object _parameter;

        public BindAnyExtension(object parameter)
        {
            _parameter = parameter;
        }

        /// <summary>
        /// Gets the framework element (target or root).
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        private static FrameworkElement GetFrameworkElement(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                return null;

            FrameworkElement frameworkElement = null;
            var provideValueTarget = serviceProvider.GetService<IProvideValueTarget>();
            if (provideValueTarget != null)
                frameworkElement = provideValueTarget.TargetObject as FrameworkElement;
            if (frameworkElement != null)
                return frameworkElement;

            var rootObjectProvider = serviceProvider.GetService<IRootObjectProvider>();
            if (rootObjectProvider != null)
                frameworkElement = (FrameworkElement)rootObjectProvider.RootObject;
            return frameworkElement;
        }

        /// <summary>
        /// En cas d'implémentation dans une classe dérivée, retourne un objet qui est défini comme valeur de la propriété cible pour cette extension de balisage.
        /// </summary>
        /// <param name="serviceProvider">Objet qui peut fournir des services pour l'extension du balisage.</param>
        /// <returns>
        /// Valeur d'objet à définir sur la propriété où l'extension est appliquée.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // If no binding, this is simple
            var binding = _parameter as Binding;
            if (binding == null)
                return _parameter;

            // Otherwise, check for target data context
            var frameworkElement = GetFrameworkElement(serviceProvider);
            if (frameworkElement == null)
                return null;

            if (!DesignerProperties.GetIsInDesignMode(frameworkElement))
            {
                // Keep target and property (they're not available later)
                var provideValueTarget = serviceProvider.GetService<IProvideValueTarget>();
                var targetProperty = (MemberInfo)provideValueTarget.TargetProperty;
                var targetObject = provideValueTarget.TargetObject;
                // And wait for changes
                frameworkElement.DataContextChanged += delegate
                {
                    var value = binding.GetValue(frameworkElement.DataContext);
                    targetProperty.SetMemberValue(targetObject, value);
                };
            }

            return binding.GetValue(frameworkElement.DataContext);
        }
    }
}
