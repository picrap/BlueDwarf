// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Geolocation.Telize
{
    using System.Net;
    using System.ServiceModel;
    using Client;

    [ServiceContract]
    public interface ITelizeApi
    {
        [OperationContract]
        [HttpGet(UriTemplate = "/geoip/{ip}")]
        TelizeAddressGeolocation GeoIP(IPAddress ip);
    }
}
