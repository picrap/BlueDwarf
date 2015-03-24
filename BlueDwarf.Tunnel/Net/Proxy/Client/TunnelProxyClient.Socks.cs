// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client
{
    using System.Net;
    using System.Net.Sockets;
    using Starksoft.Aspen.Proxy;

    partial class TunnelProxyClient
    {
        private Socket SocksProxyConnect(Socket socket, IPEndPoint target)
        {
            try
            {
                var proxyClient = new Socks4aProxyClient(new TcpClient { Client = socket });
                var tcpClient = proxyClient.CreateConnection(target.Address.ToString(), target.Port);
                return tcpClient.Client;
            }
            catch (ProxyException)
            {
                throw new ProxyRouteException(new ProxyServer(ProxyProtocol.HttpConnect, target.Address, target.Port));
            }
        }
    }
}
