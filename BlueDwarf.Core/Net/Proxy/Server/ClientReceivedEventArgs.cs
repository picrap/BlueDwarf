// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Server
{
    using System;

    /// <summary>
    /// Sent when data is received from server
    /// </summary>
    public class ClientReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Amount of data received.
        /// </summary>
        /// <value>
        /// The received.
        /// </value>
        public int Received { get; private set; }

        public ClientReceivedEventArgs(int received)
        {
            Received = received;
        }
    }
}
