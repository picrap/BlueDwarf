namespace BlueDwarf.Aspects
{
    /// <summary>
    /// Notification type for DependencyPropertyChanged
    /// </summary>
    public enum AutoDependencyPropertyNotification
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