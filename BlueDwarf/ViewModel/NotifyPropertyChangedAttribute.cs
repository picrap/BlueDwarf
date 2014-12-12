using System;

namespace BlueDwarf.ViewModel
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NotifyPropertyChangedAttribute : Attribute
    {
    }
}