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
        public AddressGeolocation Locate(IPAddress address, Route route)
        {
            var hostIPApi = Rest.Client<IHostIPApi>(route);
            var result = hostIPApi.GetJson(address);
            return new AddressGeolocation(address, result.CountryCode, result.CountryName);
        }
    }
}
