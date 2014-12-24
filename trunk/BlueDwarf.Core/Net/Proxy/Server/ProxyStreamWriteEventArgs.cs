using System;

namespace BlueDwarf.Net.Proxy.Server
{
    public class ProxyStreamWriteEventArgs : EventArgs
    {
        public int BytesWritten { get; private set; }

        public ProxyStreamWriteEventArgs(int bytesWritten)
        {
            BytesWritten = bytesWritten;
        }
    }
}