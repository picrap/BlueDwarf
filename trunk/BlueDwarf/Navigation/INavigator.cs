﻿// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Navigation
{
    using System;

    /// <summary>
    /// Navigation interface, to be injected in view-models
    /// </summary>
    public interface INavigator
    {
        /// <summary>
        /// Occurs when exiting.
        /// </summary>
        event EventHandler Exiting;

        /// <summary>
        /// Configures the specified view model type to be used with view type.
        /// </summary>
        /// <param name="viewModelType">Type of the view model.</param>
        /// <param name="viewType">Type of the view.</param>
        void Configure(Type viewModelType, Type viewType);

        /// <summary>
        /// Shows the specified view model type.
        /// </summary>
        /// <param name="viewModelType">Type of the view model.</param>
        /// <param name="initializer"></param>
        /// <returns>The view model if dialog is OK, null if cancelled</returns>
        object Show(Type viewModelType, Action<object> initializer = null);

        /// <summary>
        /// Exits the view.
        /// </summary>
        /// <param name="validate">for a dialog, true if the result has to be used</param>
        void Exit(bool validate);
    }
}
