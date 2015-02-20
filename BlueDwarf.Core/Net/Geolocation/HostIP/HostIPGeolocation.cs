// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Geolocation.HostIP
{
    using System;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using Client;
    using Geolocation;
    using Proxy.Client;

    public class HostIPGeolocation : IGeolocation
    {
        public AddressGeolocation Locate(IPAddress address, ProxyRoute proxyRoute)
        {
            var f = new RestClientFactory();
            var c = f.CreateClient<IHostIPApi>(new Uri("http://api.hostip.info"), proxyRoute);
            var r = c.GetJson(address.ToString());

            throw new NotImplementedException();
        }
    }
}
