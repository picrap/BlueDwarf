using System;

namespace BlueDwarf.Net.Proxy.Client
{
    public class ProxyClientTransferEventArgs : EventArgs
    {
        public int BytesWritten { get; private set; }
        public int BytesRead { get; private set; }

        public ProxyClientTransferEventArgs(int bytesRead, int bytesWritten)
        {
            BytesRead = bytesRead;
            BytesWritten = bytesWritten;
        }
    }
}