// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Client
{
    using System;
    using ArxOne.MrAdvice.Advice;
    using Proxy.Client;

    public static class Rest
    {
        /// <summary>
        /// Gets a client bound for specific address.
        /// </summary>
        /// <typeparam name="TClient">The type of the client.</typeparam>
        /// <param name="hostAddress">The host address.</param>
        /// <param name="proxyRoute">The proxy route.</param>
        /// <returns></returns>
        public static TClient Client<TClient>(Uri hostAddress, ProxyRoute proxyRoute)
        {
            var restAdvice = new RestAdvice(hostAddress, proxyRoute);
            return restAdvice.Handle<TClient>();
        }
    }
}
