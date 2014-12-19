using System;

namespace BlueDwarf.ViewModel
{
    /// <summary>
    /// A simple attribute to mark our view-model properties in order to have an automatic notification
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NotifyPropertyChangedAttribute : Attribute
    {
        public object Category { get; set; }
    }
}