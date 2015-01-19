// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Threading;
    using Annotations;
    using Aspects;
    using Microsoft.Practices.Unity;
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

        public void Configure(Type viewModelType, Type viewType)
        {
            _viewByViewModel[viewModelType] = viewType;
        }

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

        private object ShowDialog(Window window, ViewModel viewModel)
        {
            window.Owner = _windows.Peek();
            // the Exit() method is called only if the window is still present
            window.Closed += delegate { if (_windows.Contains(window)) Exit(false); };
            _windows.Push(window);
            var ok = window.ShowDialog();
            return ok ?? (false) ? viewModel : null;
        }

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
