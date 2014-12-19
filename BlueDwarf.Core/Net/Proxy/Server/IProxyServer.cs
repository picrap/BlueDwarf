using BlueDwarf.Net.Proxy.Client;

namespace BlueDwarf.Net.Proxy.Server
{
    /// <summary>
    /// Proxy server interface
    /// </summary>
    public interface IProxyServer
    {
        /// <summary>
        /// Gets or sets the listening port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        int Port { get; set; }

        /// <summary>
        /// Gets or sets the proxy route.
        /// </summary>
        /// <value>
        /// The proxy route.
        /// </value>
        ProxyRoute ProxyRoute { get; set; }

        /// <summary>
        /// Starts listening and serving.
        /// </summary>
        void Start();
    }
}