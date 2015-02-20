// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Geolocation
{
    using System.Net;

    /// <summary>
    /// Geolocation for given address
    /// </summary>
    public class AddressGeolocation
    {
        public IPAddress Address { get; private set; }
        public string CountryCode { get; private set; }
        public string CountryName { get; private set; }

        public AddressGeolocation(IPAddress address, string countryCode, string countryName)
        {
            Address = address;
            CountryCode = countryCode;
            CountryName = countryName;
        }
    }
}
