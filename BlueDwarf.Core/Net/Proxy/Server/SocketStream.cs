// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Server
{
    using System.IO;
    using System.Net.Sockets;
    using Annotations;

    /// <summary>
    /// Because of some twisted implementations, we had to expose the underlying socket
    /// </summary>
    public class SocketStream : NetworkStream
    {
        public new Socket Socket { get { return base.Socket; } }

        public SocketStream([NotNull] Socket socket)
            : base(socket)
        {
        }

        public SocketStream([NotNull] Socket socket, bool ownsSocket)
            : base(socket, ownsSocket)
        {
        }

        public SocketStream([NotNull] Socket socket, FileAccess access)
            : base(socket, access)
        {
        }

        public SocketStream([NotNull] Socket socket, FileAccess access, bool ownsSocket)
            : base(socket, access, ownsSocket)
        {
        }
    }
}