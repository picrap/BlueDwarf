// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Server
{
    using System;

    public class ProxyServerTransferEventArgs : EventArgs
    {
        public int BytesWritten { get; private set; }
        public int BytesRead { get; private set; }

        public ProxyServerTransferEventArgs(int bytesRead, int bytesWritten)
        {
            BytesRead = bytesRead;
            BytesWritten = bytesWritten;
        }
    }
}