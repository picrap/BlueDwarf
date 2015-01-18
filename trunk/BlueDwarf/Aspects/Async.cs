﻿using System;
using System.Reflection;
using System.Threading;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace BlueDwarf.Aspects
{
    using PostSharp.Aspects.Configuration;

    /// <summary>
    /// Allows to invoke a method asynchronously (here, in a background thread)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [MulticastAttributeUsage(MulticastTargets.Method, PersistMetaData = true)]
    [Serializable]
    [MethodInterceptionAspectConfiguration(AspectPriority = 10)]
    public class Async : Aspect, IMethodInterceptionAspect
    {
        public bool KillExisting { get; set; }

        /// <summary>
        /// Gets or sets the name of the thread (debug feature).
        /// </summary>
        /// <value>
        /// The name of the thread.
        /// </value>
        public string ThreadName { get; set; }

        [NonSerialized]
        private Thread _thread;

        public void RuntimeInitialize(MethodBase method)
        {
            var methodInfo = method as MethodInfo;
            if (methodInfo == null)
                return;
            if (methodInfo.ReturnType != typeof(void))
                throw new InvalidOperationException("Impossible to run asynchronously a non-void method (you MoFo!)");
        }

        public void OnInvoke(MethodInterceptionArgs args)
        {
            if (KillExisting && _thread != null && _thread.IsAlive)
                _thread.Abort();
            _thread = new Thread(args.Proceed) { IsBackground = true, Name = ThreadName };
            _thread.Start();
        }
    }
}
