// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Threading;
    using Annotations;
    using Microsoft.Practices.Unity;
    using View.Properties;
    using ViewModel;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    internal class Navigator : INavigator
    {
        [Dependency]
        public IUnityContainer UnityContainer { get; set; }

        [Attached]
        public static Property<Window, bool> WasShown { get; set; }

        private readonly IDictionary<Type, Type> _viewByViewModel = new Dictionary<Type, Type>();

        private readonly Stack<Window> _windows = new Stack<Window>();

        public event EventHandler Exiting;

        /// <summary>
        /// Configures the specified view model type to be used with view type.
        /// </summary>
        /// <param name="viewModelType">Type of the view model.</param>
        /// <param name="viewType">Type of the view.</param>
        public void Configure(Type viewModelType, Type viewType)
        {
            _viewByViewModel[viewModelType] = viewType;
        }

        /// <summary>
        /// Shows the specified view model type.
        /// </summary>
        /// <param name="viewModelType">Type of the view model.</param>
        /// <param name="initializer"></param>
        /// <returns>
        /// The view model if dialog is OK, null if cancelled
        /// </returns>
        public object Show(Type viewModelType, Action<object> initializer = null)
        {
            var viewModel = (ViewModel)UnityContainer.Resolve(viewModelType);
            // initializer comes first
            if (initializer != null)
                initializer(viewModel);
            // load comes second
            viewModel.Load();
            var viewType = _viewByViewModel[viewModelType];
            var view = (FrameworkElement)UnityContainer.Resolve(viewType);
            view.DataContext = viewModel;
            var window = view as Window;
            if (window != null)
            {
                if (_windows.Count == 0)
                    return ShowMain(window, viewModel);
                return ShowDialog(window, viewModel);
            }
            return null;
        }

        /// <summary>
        /// Shows the view/view-model as dialog.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        private object ShowDialog(Window window, ViewModel viewModel)
        {
            window.Owner = _windows.Peek();
            // the Exit() method is called only if the window is still present
            window.Closed += delegate { if (_windows.Contains(window)) Exit(false); };
            _windows.Push(window);
            var ok = window.ShowDialog();
            return ok ?? (false) ? viewModel : null;
        }

        /// <summary>
        /// Shows the view/view-model as main window (first window).
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        private object ShowMain(Window window, ViewModel viewModel)
        {
            _windows.Push(window);
            if (window.Visibility != Visibility.Collapsed)
            {
                window.Show();
                if (window.ShowActivated)
                    window.Activate();
            }
            window.IsVisibleChanged += OnVisibleChanged;
            return viewModel;
        }

        private static void OnVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var window = (Window)sender;
            if (!WasShown[window] && window.IsVisible)
            {
                WasShown[window] = true;
                window.Show();
                window.Activate();
            }
        }

        /// <summary>
        /// Exits the view.
        /// </summary>
        /// <param name="validate">for a dialog, true if the result has to be used</param>
        public void Exit(bool validate)
        {
            var window = _windows.Pop();
            if (_windows.Count == 0)
            {
                var onExiting = Exiting;
                if (onExiting != null)
                    onExiting(this, EventArgs.Empty);
                // this is not something I'm very proud of
                // TODO: have a nice exit
                Application.Current.DispatcherUnhandledException += delegate(object sender, DispatcherUnhandledExceptionEventArgs e) { e.Handled = true; };
                Application.Current.Shutdown();
            }
            else
            {
                window.DialogResult = validate;
                window.Close();
            }
        }
    }
}
