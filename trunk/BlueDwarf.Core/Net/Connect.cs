// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net
{
    using System.IO;
    using System.Net.Sockets;
    using System.Threading;
    using Proxy.Server;

    public static class Connect
    {
        /// <summary>
        /// Simple socket connection to specified host+port.
        /// </summary>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <returns></returns>
        public static ProxyStream To(string targetHost, int targetPort)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(targetHost, targetPort);
                    var newStream = new ProxyStream(socket, true);
                    return newStream;
                }
                catch (SocketException)
                {
                }
                catch (IOException)
                {
                }
                Thread.Sleep(1000);
            }
            return null;
        }
    }
}
