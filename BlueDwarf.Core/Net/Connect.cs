// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using Proxy.Server;
    using Utility;

    public static class Connect
    {
        /// <summary>
        /// Simple socket connection to specified host+port.
        /// </summary>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="retry">The retry.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static SocketStream To(string targetHost, int targetPort, int retry = 2, int timeout = 1000)
        {
            var timeoutTimeSpan = TimeSpan.FromMilliseconds(timeout);
            for (int i = 0; i < retry; i++)
            {
                try
                {
                    var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(targetHost, targetPort, timeoutTimeSpan);
                    var newStream = new SocketStream(socket, true);
                    return newStream;
                }
                catch (SocketException)
                {
                }
                catch (IOException)
                {
                }
            }
            return null;
        }
        /// <summary>
        /// Simple socket connection to specified address+port.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="retry">The retry.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static SocketStream To(IPAddress target, int targetPort, int retry = 2, int timeout = 1000)
        {
            var timeoutTimeSpan = TimeSpan.FromMilliseconds(timeout);
            for (int i = 0; i < retry; i++)
            {
                try
                {
                    var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(target, targetPort, timeoutTimeSpan);
                    if (socket.Connected)
                    {
                        var newStream = new SocketStream(socket, true);
                        return newStream;
                    }
                }
                catch (SocketException)
                {
                }
                catch (IOException)
                {
                }
            }
            return null;
        }
    }
}
