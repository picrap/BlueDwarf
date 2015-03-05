// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Utility
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Extensions to Socket class
    /// </summary>
    public static class SocketExtensions
    {
        /// <summary>
        /// Connects the specified socket.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <param name="timeout">The timeout.</param>
        public static void Connect(this Socket socket, string host, int port, TimeSpan timeout)
        {
            AsyncConnect(socket, (s, a, o) => s.BeginConnect(host, port, a, o), timeout);
        }

        /// <summary>
        /// Connects the specified socket.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="address">The address.</param>
        /// <param name="port">The port.</param>
        /// <param name="timeout">The timeout.</param>
        public static void Connect(this Socket socket, IPAddress address, int port, TimeSpan timeout)
        {
            AsyncConnect(socket, (s, a, o) => s.BeginConnect(address, port, a, o), timeout);
        }

        /// <summary>
        /// Asyncs the connect.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="connect">The connect.</param>
        /// <param name="timeout">The timeout.</param>
        private static void AsyncConnect(Socket socket, Func<Socket, AsyncCallback, object, IAsyncResult> connect, TimeSpan timeout)
        {
            var asyncResult = connect(socket, null, null);
            if (asyncResult.AsyncWaitHandle.WaitOne(timeout))
            {
                socket.EndConnect(asyncResult);
                return;
            }

            try
            {
                socket.Close();
            }
            catch (SocketException)
            {
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}
