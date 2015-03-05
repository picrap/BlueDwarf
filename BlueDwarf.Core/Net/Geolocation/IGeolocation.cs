// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Geolocation
{
    using System.Net;
    using Proxy.Client;

    public interface IGeolocation
    {
        AddressGeolocation Locate(IPAddress address, Route route);
    }
}
