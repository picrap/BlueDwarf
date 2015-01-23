// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using System.Linq;
    using Annotations;
    using Client;
    using Microsoft.Practices.Unity;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class ProxyValidator : IProxyValidator
    {
        [Dependency]
        public IProxyClient ProxyClient { get; set; }

        /// <summary>
        /// Validates the specified proxy host port as a HTTP CONNECT proxy.
        /// </summary>
        /// <param name="proxyHostPort">The proxy host port.</param>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="routeToProxy">The route to proxy.</param>
        /// <returns></returns>
        public bool Validate(HostPort proxyHostPort, string targetHost, int targetPort, ProxyRoute routeToProxy)
        {
            var proxyUri = new Uri(string.Format("http://{0}:{1}", proxyHostPort.Host ?? proxyHostPort.Address.ToString(), proxyHostPort.Port));
            try
            {
                return ProxyClient.CreateRoute(targetHost, targetPort, routeToProxy.Route.Concat(new[] { proxyUri }).ToArray()) != null;
            }
            catch (ProxyRouteException)
            {
            }
            return false;
        }
    }
}
