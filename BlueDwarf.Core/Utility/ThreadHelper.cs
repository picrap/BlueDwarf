
using System;
using System.Threading;

namespace BlueDwarf.Utility
{
    public static class ThreadHelper
    {
        /// <summary>
        /// Thread for dummies (like me).
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static Thread CreateBackground(Action action)
        {
            ThreadStart threadStart = () => action();
            var thread = new Thread(threadStart) { IsBackground = true };
            thread.Start();
            return thread;
        }
    }
}
