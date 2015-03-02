// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Controls
{
    using System;
    using System.Windows.Markup;
    using System.Xaml;
    using Utility;

    public  class LocaleExtension: MarkupExtension
    {
        private readonly object _parameter;

        public LocaleExtension(object parameter)
        {
            _parameter = parameter;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValueTarget = serviceProvider.GetService<IProvideValueTarget>();
            var rootObjectProvider = serviceProvider.GetService<IRootObjectProvider>();
            return "hop";
        }
    }
}
