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
        /// <param name="route">The route.</param>
        /// <param name="hostAddress">The host address.</param>
        /// <returns></returns>
        public static TClient Client<TClient>(Route route, Uri hostAddress = null)
        {
            var restAdvice = new RestAdvice(hostAddress, route);
            return restAdvice.Handle<TClient>();
        }
    }
}
