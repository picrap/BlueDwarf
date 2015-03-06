namespace ArxOne.MrAdvice.MVVM.View
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Input;

    internal class Command : ICommand
    {
        private readonly object _viewModel;
        private MethodBase _commandMethod;

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command" /> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="parameter">The parameter.</param>
        public Command(object viewModel, object parameter)
        {
            _viewModel = viewModel;
            SetCommand(parameter);
        }

        private void SetCommand(object parameter)
        {
            _commandMethod = parameter as MethodBase;
            if (_commandMethod != null)
                return;

            var commandString = (string)parameter;
            _commandMethod = _viewModel.GetType().GetMethod(commandString);
            if (_commandMethod == null)
                throw new InvalidOperationException(string.Format("Command '{0}' not found", commandString));
        }

        public bool CanExecute(object parameter)
        {
            // TODO :)
            return true;
        }

        public void Execute(object parameter)
        {
            var parameters = new List<object>();
            if (_commandMethod.GetParameters().Length > 0)
                parameters.Add(parameter);
            _commandMethod.Invoke(_viewModel, parameters.ToArray());
        }
    }
}
