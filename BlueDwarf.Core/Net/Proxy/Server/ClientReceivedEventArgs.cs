// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Server
{
    using System;

    public class ClientReceivedEventArgs : EventArgs
    {
        public int Received { get; private set; }

        public ClientReceivedEventArgs(int received)
        {
            Received = received;
        }
    }
}
