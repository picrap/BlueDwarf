namespace BlueDwarf
{
    /// <summary>
    /// Navigation interface, to be injected in view-models
    /// </summary>
    public interface INavigator
    {
        /// <summary>
        /// Exits the application.
        /// </summary>
        void Exit();
    }
}
