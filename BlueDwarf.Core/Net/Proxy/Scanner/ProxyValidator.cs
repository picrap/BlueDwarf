// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using Annotations;
    using Client;
    using Http;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class ProxyValidator : IProxyValidator
    {
        /// <summary>
        /// Validates the specified proxy host port as a HTTP CONNECT proxy.
        /// </summary>
        /// <param name="proxyHostPort">The proxy host port.</param>
        /// <param name="testTarget"></param>
        /// <param name="routeToProxy"></param>
        /// <param name="tryCount"></param>
        /// <returns></returns>
        public bool ValidateHttpConnect(HostPort proxyHostPort, Uri testTarget, Route routeToProxy, int tryCount)
        {
            var proxyUri = new Uri(string.Format("http://{0}:{1}", proxyHostPort.Host ?? proxyHostPort.Address.ToString(), proxyHostPort.Port));
            try
            {
                Validate(routeToProxy + proxyUri, testTarget, tryCount);
                return true;
            }
            catch (ProxyRouteException)
            {
                return false;
            }
        }

        /// <summary>
        /// Validates the specified route, using the test target.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="testTarget">The test target.</param>
        /// <param name="tryCount"></param>
        /// <returns></returns>
        public void Validate(Route route, Uri testTarget, int tryCount)
        {
            for (int tryIndex = 0; tryIndex < tryCount; tryIndex++)
            {
                // result does not matter
                using (var httpStream = route.Connect(testTarget))
                {
                    HttpRequest.CreateGet(testTarget).Write(httpStream);
                    var httpResponse = HttpResponse.FromStream(httpStream);
                    if (httpResponse.StatusCode > 0 && httpResponse.StatusCode < 400)
                        continue;
#if DEBUG
                    var content = httpResponse.ReadContentString(httpStream);
#endif
                    throw new ProxyRouteException(testTarget.Host);
                }
            }
        }
    }
}
