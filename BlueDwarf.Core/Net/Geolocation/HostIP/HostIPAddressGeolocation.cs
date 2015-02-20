// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Geolocation.HostIP
{
    using System.Runtime.Serialization;

    [DataContract]
    public class HostIPAddressGeolocation
    {
        [DataMember(Name = "ip")]
        public string Address { get; set; }
        [DataMember(Name = "country_code")]
        public string CountryCode { get; set; }
        [DataMember(Name = "country_name")]
        public string CountryName { get; set; }
    }
}
