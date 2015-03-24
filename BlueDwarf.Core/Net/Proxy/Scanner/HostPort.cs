// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Proxy.Scanner
{
    public static class HostPort 
    {
        // A literal decimal byte, from 0 to 255
        private const string ByteRangeEx = @"(\d{1,2}|([0-1]\d{2})|(2[0-4]\d)|(25[0-5]))";
        // so an IP v4 is 4 literal bytes, separated by a dot
        public const string IPv4Ex = @"(?<address>(" + ByteRangeEx + @"\." + ByteRangeEx + @"\." + ByteRangeEx + @"\." + ByteRangeEx + @"))";
        // A port from 0 to 65535
        public const string PortEx = @"(?<port>(([0-5]\d{4})|(6[0-4]\d{3})|(65[0-4]\d{2})|(655[0-2]\d)|(6553[0-5])|\d{1,4}))";
    }
}
