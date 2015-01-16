using System;

namespace BlueDwarf.Net.Proxy.Server
{
    public class ClientReceivedEventArgs : EventArgs
    {
        public int Received { get; private set; }

        public ClientReceivedEventArgs(int received)
        {
            Received = received;
        }
    }
}
