using System;
using System.Windows.Threading;

namespace BlueDwarf.Utility
{
    /// <summary>
    /// Simple extensions to Dispatcher.
    /// </summary>
    public static class DispatcherExtensions
    {
        /// <summary>
        /// Invokes the specified action.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="action">The action.</param>
        public static void Invoke(this Dispatcher dispatcher, Action action)
        {
            dispatcher.Invoke((Delegate)action);
        }

        /// <summary>
        /// Invokes the specified function.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        public static TResult Invoke<TResult>(this Dispatcher dispatcher, Func<TResult> func)
        {
            return (TResult)dispatcher.Invoke((Delegate)func);
        }
    }
}
