// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Name.StatDns
{
    using System.ServiceModel;
    using Client;

    [ServiceContract(Namespace = "http://api.statdns.com")]
    public interface IStatDns
    {
        [OperationContract]
        [HttpGet(UriTemplate = "/{name}/{type}")]
        Response Ask(string name, string type);
    }
}
