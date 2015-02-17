// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Client
{
    using Http;
    using Server;

    partial class TunnelProxyClient
    {
        /// <summary>
        /// Proxy connection using HTTP.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <returns></returns>
        private ProxyStream HttpProxyConnect(ProxyStream stream, string targetHost, int targetPort)
        {
            new HttpRequest("CONNECT", targetHost, targetPort).AddHeader("Proxy-Connection", "Keep-Alive").Write(stream);
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
