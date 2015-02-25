// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.ViewModel.Properties
{
    /// <summary>
    /// Notification type for DependencyPropertyChanged
    /// </summary>
    public enum DependencyPropertyNotification
    {
        /// <summary>
        /// No notification (this is the default)
        /// </summary>
        None,

        /// <summary>
        /// Invokes a notification method named "On&lt;PropertyName>Changed"
        /// </summary>
        OnPropertyNameChanged,
    }
}