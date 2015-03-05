// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client
{
    using System.Net;
    using Http;
    using Server;

    partial class TunnelProxyClient
    {
        /// <summary>
        /// Proxy connection using HTTP.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetPort">The target port.</param>
        /// <returns></returns>
        private SocketStream HttpProxyConnect(SocketStream stream, IPAddress target, int targetPort)
        {
            new HttpRequest("CONNECT", target.ToString(), targetPort).AddHeader("Proxy-Connection", "Keep-Alive").Write(stream);
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
            return stream;
        }
    }
}
