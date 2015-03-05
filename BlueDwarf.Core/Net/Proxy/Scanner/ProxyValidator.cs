// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using Annotations;
    using Client;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class ProxyValidator : IProxyValidator
    {
        /// <summary>
        /// Validates the specified proxy host port as a HTTP CONNECT proxy.
        /// </summary>
        /// <param name="proxyHostPort">The proxy host port.</param>
        /// <param name="testTargetHost"></param>
        /// <param name="testTargetPort"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        public bool ValidateHttpConnect(HostPort proxyHostPort, string testTargetHost, int testTargetPort, Route route)
        {
            var proxyUri = new Uri(string.Format("http://{0}:{1}", proxyHostPort.Host ?? proxyHostPort.Address.ToString(), proxyHostPort.Port));
            try
            {
                // validation is simple: if route creation succeeds, then the proxy is valid
                var newRoute = route + proxyUri;
                // we don't care much about connexion
                using (newRoute.Connect(testTargetHost, testTargetPort))
                {
                }
                return true;
            }
            catch (ProxyRouteException)
            {
            }
            return false;
        }
    }
}
