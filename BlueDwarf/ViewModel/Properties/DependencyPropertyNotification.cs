// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

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