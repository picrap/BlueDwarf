
namespace ArxOne.MrAdvice.MVVM.ViewModel
{
    using System.ComponentModel;
    using System.Reflection;
    using Properties;

    public interface INotifyPropertyChangedViewModel : INotifyPropertyChanged
    {
        void OnPropertyChanged(PropertyInfo propertyInfo, NotifyPropertyChanged sender);
    }
}
