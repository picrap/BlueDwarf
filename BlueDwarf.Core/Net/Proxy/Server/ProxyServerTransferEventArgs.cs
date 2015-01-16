using System;

namespace BlueDwarf.Net.Proxy.Server
{
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