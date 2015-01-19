// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Server
{
    using System.IO;
    using System.Net.Sockets;
    using Annotations;

    /// <summary>
    /// Because of some twisted implementations, we had to expose the underlying socket
    /// </summary>
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