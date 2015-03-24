// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using System.IO;
    using Annotations;
    using Client;
    using Http;
    using Microsoft.Practices.Unity;
    using Name;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class ProxyValidator : IProxyValidator
    {
        /// <summary>
        /// Gets or sets the name resolver.
        /// </summary>
        /// <value>
        /// The name resolver.
        /// </value>
        [Dependency]
        public INameResolver NameResolver { get; set; }

        /// <summary>
        /// Validates the specified proxy host port as a HTTP CONNECT proxy.
        /// </summary>
        /// <param name="proxyServer"></param>
        /// <param name="testTarget"></param>
        /// <param name="routeToProxy"></param>
        /// <param name="tryCount"></param>
        /// <returns></returns>
        public bool Validate(ProxyServer proxyServer, Uri testTarget, Route routeToProxy, int tryCount = 3)
        {
            try
            {
                Validate(routeToProxy + proxyServer, testTarget, tryCount);
                return true;
            }
            catch (IOException)
            {
            }
            catch (ProxyRouteException)
            {
            }
            return false;
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
                using (var httpStream = route.Connect(testTarget, NameResolver))
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
