// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Controls
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;
    using Utility;
    using ViewModel;

    /// <summary>
    /// Allows to bind commands directly to view-model methods
    /// Syntax: Command="{controls:Command {Binding methodName}}"
    /// </summary>
    public class CommandExtension : MarkupExtension
    {
        private readonly object _parameter;

        public CommandExtension(object parameter)
        {
            _parameter = parameter;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValueTarget = serviceProvider.GetService<IProvideValueTarget>();
            var element = provideValueTarget.TargetObject as FrameworkElement;
            if (element == null)
                return null;

            var targetProperty = provideValueTarget.TargetProperty;
            element.DataContextChanged += delegate
            {
                var viewModel = element.DataContext as ViewModel;
                if (viewModel == null)
                    return;

                var parameter = _parameter;
                var bindingParameter = parameter as Binding;
                // because we bind to a method, this allows us to have a syntax control in XAML editor
                if (bindingParameter != null)
                    parameter = viewModel.GetType().GetMember(bindingParameter.Path.Path).FirstOrDefault();

                element.SetCommand(targetProperty, viewModel, parameter);
            };

            return null;
        }
    }
}
