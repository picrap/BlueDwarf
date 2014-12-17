
using System.IO;
using System.Net.Sockets;
using System.Threading;
using BlueDwarf.Net.Proxy.Server;

namespace BlueDwarf.Net
{
    public static class Connect
    {
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
