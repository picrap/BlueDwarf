using System;
using System.IO;
using System.Net.Sockets;
using BlueDwarf.Annotations;
using BlueDwarf.Utility;

namespace BlueDwarf.Net.Proxy.Server
{
    /// <summary>
    /// Because of some twisted implementations, we had to expose the underlying socket
    /// </summary>
    public class ProxyStream : NetworkStream
    {
        public new Socket Socket { get { return base.Socket; } }

        public event EventHandler<ProxyStreamReadEventArgs> DataRead;
        public event EventHandler<ProxyStreamWriteEventArgs> DataWritten;

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

        public override int Read(byte[] buffer, int offset, int size)
        {
            var bytesRead = base.Read(buffer, offset, size);
            DataRead.Raise(this, new ProxyStreamReadEventArgs(bytesRead));
            return bytesRead;
        }

        public override void Write(byte[] buffer, int offset, int size)
        {
            base.Write(buffer, offset, size);
            DataWritten.Raise(this, new ProxyStreamWriteEventArgs(size));
        }
    }
}