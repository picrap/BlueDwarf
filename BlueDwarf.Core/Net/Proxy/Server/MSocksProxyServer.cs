// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Server
{
    using System;
    using System.Net.Sockets;
    using Client;
    using MSocksServer.Socks4Server;
    using Utility;

    /// <summary>
    /// Socks proxy server
    /// Was from far the easiest to implement
    /// </summary>
    internal class MSocksProxyServer : IProxyServer
    {
        private Socks4 _socksServer;

        /// <summary>
        /// Occurs when a client connects.
        /// </summary>
        public event EventHandler Connect;
        /// <summary>
        /// Occurs when data is transferred, from or to client.
        /// </summary>
        public event EventHandler<ProxyServerTransferEventArgs> Transfer;

        private int? _port;
        public int? Port
        {
            get { return _port; }
            set
            {
                if (_port != value)
                {
                    _port = value;
                    ConfigureServer();
                }
            }
        }

        /// <summary>
        /// Gets or sets the proxy route.
        /// </summary>
        /// <value>
        /// The proxy route.
        /// </value>
        public Route Route { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MSocksProxyServer" /> class.
        /// </summary>
        public MSocksProxyServer()
        {
        }

        /// <summary>
        /// Disposes the instance.
        /// </summary>
        public void Dispose()
        {
            // By setting the port to null, we release the current server (if it exists) and do not start another one
            Port = null;
        }

        private void ConfigureServer()
        {
            if (_socksServer != null)
            {
                _socksServer.Stop();
                _socksServer = null;
            }
            if (Port.HasValue)
            {
                if (Port.Value > 0)
                {
                    _socksServer = CreateListener(Port.Value);
                }
                else
                {
                    for (; ; )
                    {
                        try
                        {
                            _socksServer = CreateListener(0);
                            _port = _socksServer.Port;
                            break;
                        }
                        catch (SocketException)
                        {
                        }
                    }
                }
            }
        }

        private Socks4 CreateListener(int listeningPort)
        {
            var server = new Socks4(listeningPort, ClientConnect);
            server.OnSendData += OnSendData;
            server.OnReceiveData += OnReceiveData;
            server.Start();
            return server;
        }

        /// <summary>
        /// Connects to host+port, using the current route.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        private Socket ClientConnect(string host, int port)
        {
            Connect.Raise(this);
            return Route.Connect(host, port);
        }

        private void OnReceiveData(ref byte[] data, ref bool blocked, Socks4ThreadInfo info)
        {
            Transfer.Raise(this, new ProxyServerTransferEventArgs(data.Length, 0));
        }

        private void OnSendData(ref byte[] data, ref bool blocked, Socks4ThreadInfo info)
        {
            Transfer.Raise(this, new ProxyServerTransferEventArgs(0, data.Length));
        }
    }
}
