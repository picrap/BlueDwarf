// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Geolocation.HostIP
{
    using System;
    using System.Net;
    using Client;
    using Geolocation;
    using Proxy.Client;

    /// <summary>
    /// Geolocation implementation with HostIP
    /// </summary>
    public class HostIPGeolocation : IGeolocation
    {
        public AddressGeolocation Locate(IPAddress address, ProxyRoute proxyRoute)
        {
            var hostIPApi = Rest.Client<IHostIPApi>(new Uri("http://api.hostip.info"), proxyRoute);
            var result = hostIPApi.GetJson(address);
            return new AddressGeolocation(address, result.CountryCode, result.CountryName);
        }
    }
}
