// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Geolocation.HostIP
{
    using System.Net;
    using System.ServiceModel;
    using Client;

    [ServiceContract(Namespace = "http://api.hostip.info")]
    public interface IHostIPApi
    {
        [OperationContract]
        [HttpGet(UriTemplate = "/get_json.php?ip={ip}")]
        HostIPAddressGeolocation GetJson(IPAddress ip);
    }
}
