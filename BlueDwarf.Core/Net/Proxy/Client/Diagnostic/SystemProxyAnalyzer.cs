// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client.Diagnostic
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using Annotations;
    using Http;

    /// <summary>
    /// Proxy analyzer implementation
    /// This analyzes a single proxy (the system proxy)
    /// </summary>
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    internal class SystemProxyAnalyzer : ISystemProxyAnalyzer
    {
        /// <summary>
        /// Diagnoses the system proxy.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public ProxyDiagnostic Diagnose(SystemAnalysisParameters parameters = null)
        {
            parameters = parameters ?? new SystemAnalysisParameters();
            return new ProxyDiagnostic
            {
                DefaultProxy = GetDefaultProxy(parameters.SafeHttpTarget),
                SafeHttpGetRoute = DiagnoseRoute(parameters.SafeHttpTarget, false),
                SafeHttpsConnectRoute = DiagnoseRoute(parameters.SafeHttpsTarget, true),
                SafeHttpConnectRoute = DiagnoseRoute(parameters.SafeHttpTarget, true),
                SensitiveHttpGetRoute = DiagnoseRoute(parameters.SensitiveHttpTarget, false),
                SensitiveHttpsConnectRoute = DiagnoseRoute(parameters.SensitiveHttpsTarget, true),
                SensitiveHttpConnectRoute = DiagnoseRoute(parameters.SensitiveHttpTarget, true),
                SafeLocalDns = DiagnoseDns(parameters.SafeHttpTarget.Host),
                SensitiveLocalDns = DiagnoseDns(parameters.SensitiveHttpTarget.Host),
            };
        }

        /// <summary>
        /// Gets the default proxy.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        private static Uri GetDefaultProxy(Uri uri)
        {
            var webProxy = WebRequest.DefaultWebProxy;
            if (webProxy.IsBypassed(uri))
                return null;
            var route = webProxy.GetProxy(uri);
            if (route == uri)
                return null;

            return route;
        }

        /// <summary>
        /// Diagnoses the route.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="connect">if set to <c>true</c> [connect].</param>
        /// <returns></returns>
        private RouteStatus DiagnoseRoute(Uri uri, bool connect)
        {
            var webProxy = WebRequest.DefaultWebProxy;
            if (webProxy.IsBypassed(uri))
                return 0;
            var route = webProxy.GetProxy(uri);
            if (route == uri)
                return 0;

            var status = RouteStatus.HasProxy;
            var host = uri.Host;
            if (DiagnoseRoute(connect, route, host, uri.Port))
                status |= RouteStatus.ProxyAcceptsName;
            var address = ResolveDns(host);
            if (address != null && DiagnoseRoute(connect, route, address.ToString(), uri.Port))
                status |= RouteStatus.ProxyAcceptsAddress;

            return status;
        }

        /// <summary>
        /// Diagnoses the route.
        /// </summary>
        /// <param name="connect">if set to <c>true</c> [connect].</param>
        /// <param name="route">The route.</param>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        private static bool DiagnoseRoute(bool connect, Uri route, string host, int port)
        {
            try
            {
                using (var socket = Connect.To(route.Host, route.Port))
                using (var stream = new NetworkStream(socket, true))
                {
                    if (connect)
                        new HttpRequest("CONNECT", string.Format("{0}:{1}", host, port)).Write(stream);
                    else
                        new HttpRequest("GET", "/").AddHeader("Host", string.Format("{0}:{1}", host, port)).Write(stream);

                    var response = HttpResponse.FromStream(stream);
                    if (response.StatusCode != 403)
                        return true;
                }
            }
            catch (IOException) { }
            catch (SocketException) { }
            return false;
        }

        /// <summary>
        /// Diagnoses the DNS for the given host (DNS censorship).
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        private static bool DiagnoseDns(string host)
        {
            return ResolveDns(host) != null;
        }

        private static IPAddress ResolveDns(string host)
        {
            try
            {
                var entry = Dns.GetHostEntry(host);
                return entry.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork || a.AddressFamily == AddressFamily.InterNetworkV6);
            }
            catch (SocketException) { }
            return null;
        }
    }
}