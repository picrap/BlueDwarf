// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Server
{
    using System;
    using Client;

    /// <summary>
    /// Proxy server interface
    /// </summary>
    public interface IProxyServer: IDisposable
    {
        /// <summary>
        /// Occurs when a client connects.
        /// </summary>
        event EventHandler Connect;

        /// <summary>
        /// Occurs when data is transferred, from or to client.
        /// </summary>
        event EventHandler<ProxyServerTransferEventArgs> Transfer;

        /// <summary>
        /// Gets or sets the listening port.
        /// 0 to auto-select
        /// null to stop
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        int? Port { get; set; }

        /// <summary>
        /// Gets or sets the proxy outgoing route.
        /// </summary>
        /// <value>
        /// The proxy route.
        /// </value>
        ProxyRoute ProxyRoute { get; set; }
    }
}