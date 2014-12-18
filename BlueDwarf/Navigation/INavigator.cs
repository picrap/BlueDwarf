using System;

namespace BlueDwarf.Navigation
{
    /// <summary>
    /// Navigation interface, to be injected in view-models
    /// </summary>
    public interface INavigator
    {
        /// <summary>
        /// Configures the specified view model type to be used with view type.
        /// </summary>
        /// <param name="viewModelType">Type of the view model.</param>
        /// <param name="viewType">Type of the view.</param>
        void Configure(Type viewModelType,Type viewType);
        /// <summary>
        /// Shows the specified view model type.
        /// </summary>
        /// <param name="viewModelType">Type of the view model.</param>
        /// <returns>The view model if dialog is OK, null if cancelled</returns>
        object Show(Type viewModelType);

        /// <summary>
        /// Exits the view.
        /// </summary>
        /// <param name="validate">for a dialog, true if the result has to be used</param>
        void Exit(bool validate);
    }
}
