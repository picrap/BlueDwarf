// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Geolocation.HostIP
{
    using System.ServiceModel;
    using Client;

    [ServiceContract]
    public interface IHostIPApi
    {
        [OperationContract]
        [HttpGet(UriTemplate = "/get_json.php?ip={ip}")]
        HostIPAddressGeolocation GetJson(string ip);
    }
}
