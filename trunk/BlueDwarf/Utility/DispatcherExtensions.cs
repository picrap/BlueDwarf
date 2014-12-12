using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace BlueDwarf.Utility
{
    public static class DispatcherExtensions
    {
        public static void Invoke(this Dispatcher dispatcher, Action action)
        {
            dispatcher.Invoke((Delegate)action);
        }
        public static TResult Invoke<TResult>(this Dispatcher dispatcher, Func<TResult> func)
        {
            return (TResult)dispatcher.Invoke((Delegate)func);
        }
    }
}
