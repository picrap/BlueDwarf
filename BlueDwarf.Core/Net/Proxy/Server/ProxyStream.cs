using System.IO;
using System.Net.Sockets;
using BlueDwarf.Annotations;

namespace BlueDwarf.Net.Proxy.Server
{
    public class ProxyStream : NetworkStream
    {
        public new Socket Socket { get { return base.Socket; } }

        public ProxyStream([NotNull] Socket socket)
            : base(socket)
        {
        }

        public ProxyStream([NotNull] Socket socket, bool ownsSocket)
            : base(socket, ownsSocket)
        {
        }

        public ProxyStream([NotNull] Socket socket, FileAccess access)
            : base(socket, access)
        {
        }

        public ProxyStream([NotNull] Socket socket, FileAccess access, bool ownsSocket)
            : base(socket, access, ownsSocket)
        {
        }
    }
}