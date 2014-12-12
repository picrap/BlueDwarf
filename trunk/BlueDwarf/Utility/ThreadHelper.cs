
using System;
using System.Threading;

namespace BlueDwarf.Utility
{
    public static class ThreadHelper
    {
        public static Thread Create(Action action)
        {
            ThreadStart threadStart = () => action();
            var thread = new Thread(threadStart) { IsBackground = true };
            thread.Start();
            return thread;
        }
    }
}
