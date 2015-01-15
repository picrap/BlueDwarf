using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using PostSharp.Aspects;

namespace BlueDwarf.Aspects
{
    /// <summary>
    /// Allows to invoke a method asynchronously (here, in a background thread)
    /// </summary>
    [Serializable]
    public class Async : MethodInterceptionAspect, ISerializable
    {
        public bool KillExisting { get; set; }

        private Thread _thread;

        public Async()
        { }

        public override void RuntimeInitialize(MethodBase method)
        {
            var methodInfo = method as MethodInfo;
            if (methodInfo == null)
                return;
            if (methodInfo.ReturnType != typeof(void))
                throw new InvalidOperationException("Impossible to run asynchronously a non-void method (you MoFo!)");
        }

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            if (KillExisting && _thread != null && _thread.IsAlive)
                _thread.Abort();
            _thread = new Thread(args.Proceed) { IsBackground = true };
            _thread.Start();
        }

        #region Serialization plumbing

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("k", KillExisting);
        }

        protected Async(SerializationInfo info, StreamingContext context)
        {
            KillExisting = info.GetBoolean("k");
        }

        #endregion
    }
}
