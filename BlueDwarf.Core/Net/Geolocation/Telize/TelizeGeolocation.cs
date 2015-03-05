// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Geolocation.Telize
{
    using System;
    using System.Net;
    using Annotations;
    using Client;
    using Proxy.Client;

    /// <summary>
    /// Geolocation using Telize
    /// </summary>
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    internal class TelizeGeolocation : IGeolocation
    {
      public AddressGeolocation Locate(IPAddress address, Route route)
      {
          var telizeApi = Rest.Client<ITelizeApi>(route);
          var result = telizeApi.GeoIP(address);
          return new AddressGeolocation(address, result.CountryCode, result.Country);
      }
    }
}
