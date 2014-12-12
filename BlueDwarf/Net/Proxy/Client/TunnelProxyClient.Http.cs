using BlueDwarf.Net.Http;
using BlueDwarf.Net.Proxy.Server;

namespace BlueDwarf.Net.Proxy.Client
{
    partial class TunnelProxyClient
    {
        private ProxyStream HttpProxyConnect(ProxyStream stream, string targetHost, int targetPort, ProxyRoute routeUntilHere)
        {
            new HttpRequest("CONNECT", targetHost, targetPort).AddHeader("Proxy-Connection", "Keep-Alive").Write(stream);
            var httpResponse = new HttpResponse().Read(stream);
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
