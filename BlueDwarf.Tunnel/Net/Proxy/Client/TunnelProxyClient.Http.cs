// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client
{
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using Http;
    using Starksoft.Aspen.Proxy;

    partial class TunnelProxyClient
    {
        /// <summary>
        /// Proxy connection using HTTP.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        private Socket HttpProxyConnect(Socket socket, IPEndPoint target)
        {
            var proxyClient = new HttpProxyClient(new TcpClient { Client = socket });
            var tcpClient = proxyClient.CreateConnection(target.Address.ToString(), target.Port);
            return tcpClient.Client;

            try
            {
                using (var stream = new NetworkStream(socket, false))
                {
                    HttpRequest.CreateConnect(target.Address.ToString(), target.Port).Write(stream);
                    var httpResponse = HttpResponse.FromStream(stream);
                    if (httpResponse.StatusCode != 200)
                    {
#if DEBUG
                        var content = httpResponse.ReadContentString(stream);
#endif
                        //var bc = new BlueCoatHttpAuthentication();
                        //bc.Handle(stream, httpResponse, content, networkCredential, routeUntilHere);
                        return null;
                    }
                    return socket;
                }
            }
            catch (IOException)
            { }
            return null;
        }
    }
}
