
using System;

namespace BlueDwarf.Utility
{
    public static class EventHandlerExtensions
    {
        public static void Raise<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs eventArgs)
            where TEventArgs : EventArgs
        {
            if (handler != null)
                handler(sender, eventArgs);
        }
    }
}
