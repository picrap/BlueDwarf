// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Geolocation.Telize
{
    using System;
    using System.Net;
    using Client;
    using Proxy.Client;

    /// <summary>
    /// Geolocation using Telize
    /// </summary>
    internal  class TelizeGeolocation: IGeolocation
    {
      public AddressGeolocation Locate(IPAddress address, ProxyRoute proxyRoute)
      {
          var telizeApi = Rest.Client<ITelizeApi>(new Uri("http://www.telize.com/"), proxyRoute);
          var result = telizeApi.GeoIP(address);
          return new AddressGeolocation(address, result.CountryCode, result.Country);
      }
    }
}
