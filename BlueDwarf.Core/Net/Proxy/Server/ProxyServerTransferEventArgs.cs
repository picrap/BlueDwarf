// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Server
{
    using System;

    /// <summary>
    /// Sent when data is exchanged from client to proxy server
    /// </summary>
    public class ProxyServerTransferEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the bytes written.
        /// </summary>
        /// <value>
        /// The bytes written.
        /// </value>
        public int BytesWritten { get; private set; }
        /// <summary>
        /// Gets the bytes read.
        /// </summary>
        /// <value>
        /// The bytes read.
        /// </value>
        public int BytesRead { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyServerTransferEventArgs"/> class.
        /// </summary>
        /// <param name="bytesRead">The bytes read.</param>
        /// <param name="bytesWritten">The bytes written.</param>
        public ProxyServerTransferEventArgs(int bytesRead, int bytesWritten)
        {
            BytesRead = bytesRead;
            BytesWritten = bytesWritten;
        }
    }
}