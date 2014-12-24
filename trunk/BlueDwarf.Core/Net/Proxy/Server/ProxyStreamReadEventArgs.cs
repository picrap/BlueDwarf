using System;

namespace BlueDwarf.Net.Proxy.Server
{
    public class ProxyStreamReadEventArgs : EventArgs
    {
        public int BytesRead { get; private set; }

        public ProxyStreamReadEventArgs(int bytesRead)
        {
            BytesRead = bytesRead;
        }
    }
}