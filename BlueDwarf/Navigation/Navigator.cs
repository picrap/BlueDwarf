
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using BlueDwarf.Annotations;
using BlueDwarf.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace BlueDwarf.Navigation
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    internal class Navigator : INavigator
    {
        [Dependency]
        public IUnityContainer UnityContainer { get; set; }

        private readonly IDictionary<Type, Type> _viewByViewModel = new Dictionary<Type, Type>();

        private readonly Stack<Window> _windows = new Stack<Window>();

        public void Configure(Type viewModelType, Type viewType)
        {
            _viewByViewModel[viewModelType] = viewType;
            UnityContainer.RegisterType(viewModelType, viewModelType, AsViewModel());
        }

        private static InjectionMember[] AsViewModel()
        {
            return new InjectionMember[] { new Interceptor<VirtualMethodInterceptor>(), new InterceptionBehavior<NotifyPropertyChangedBehavior>() };
        }

        public object Show(Type viewModelType, Action<object> initializer = null)
        {
            var viewModel = (ViewModel.ViewModel)UnityContainer.Resolve(viewModelType);
            // initializer comes first
            if (initializer != null)
                initializer(viewModel);
            // load comes second
            viewModel.Load();
            var viewType = _viewByViewModel[viewModelType];
            var view = (FrameworkElement)Activator.CreateInstance(viewType);
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

        private object ShowDialog(Window window, ViewModel.ViewModel viewModel)
        {
            window.Owner = _windows.Peek();
            // the Exit() method is called only if the window is still present
            window.Closed += delegate { if (_windows.Contains(window)) Exit(false); };
            _windows.Push(window);
            var ok = window.ShowDialog();
            return ok ?? (false) ? viewModel : null;
        }

        private object ShowMain(Window window, ViewModel.ViewModel viewModel)
        {
            _windows.Push(window);
            window.Show();
            window.Activate();
            return viewModel;
        }

        public void Exit(bool validate)
        {
            var window = _windows.Pop();
            if (_windows.Count == 0)
            {
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
