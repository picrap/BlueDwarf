// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using Annotations;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class ProxyPageProvider
    {
        public string Name { get; private set; }
        public Uri PageUri { get; private set; }
        public bool ParseAsText { get; private set; }
        public string HostPortEx { get; private set; }

        public ProxyPageProvider(Uri pageUri, string name, bool parseAsText, string hostPortEx)
        {
            Name = name;
            PageUri = pageUri;
            ParseAsText = parseAsText;
            HostPortEx = hostPortEx;
        }

        private static string TagEx(string tag, string innerEx)
        {
            return string.Format(@"\<{0}[^>]*\>\s*({1})\s*\<\/{0}\>", tag.Replace(":", @"\:"), innerEx);
        }

        internal static readonly string ProxynovaEx = TagEx("span", HostPort.IPv4Ex) + ".*?" + TagEx("a", HostPort.PortEx);

        internal static readonly string XroxyRssEx = TagEx("prx:ip", HostPort.IPv4Ex) + @"\s*" + TagEx("prx:port", HostPort.PortEx)
                                                     + @"\s*" + TagEx("prx:type", "Transparent|Anonymous") + @"\s*" + TagEx("prx:ssl", "true");

        private const string ProxynovaPage = "Proxynova: ";

        public static readonly ProxyPageProvider[] Default = 
        {
            new ProxyPageProvider(new Uri("http://www.xroxy.com/proxyrss.xml"), "Xroxy (RSS)", false, XroxyRssEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/"), "Proxynova (worldwide, recent checks)", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-ar/"), ProxynovaPage + "Argentina", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-br/"), ProxynovaPage + "Brazil", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-bd/"), ProxynovaPage + "Bangladesh", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-cl/"), ProxynovaPage + "Chile", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-cn/"), ProxynovaPage + "China", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-co/"), ProxynovaPage + "Colombia", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-fr/"), ProxynovaPage + "France", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-de/"), ProxynovaPage + "Germany", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-hk/"), ProxynovaPage + "Hong Kong", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-in/"), ProxynovaPage + "India", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-id/"), ProxynovaPage + "Indonesia", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-jp/"), ProxynovaPage + "Japan", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-my/"), ProxynovaPage + "Malaysia", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-ma/"), ProxynovaPage + "Morocco", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-pe/"), ProxynovaPage + "Peru", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-pl/"), ProxynovaPage + "Poland", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-ru/"), ProxynovaPage + "Russia", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-tw/"), ProxynovaPage + "Taiwan", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-th/"), ProxynovaPage + "Thailand", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-ua/"), ProxynovaPage + "Ukraine", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-gb/"), ProxynovaPage + "United Kingdom", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-us/"), ProxynovaPage + "United States", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-ve/"), ProxynovaPage + "Venezuela", false, ProxynovaEx),
            new ProxyPageProvider(new Uri("http://www.proxynova.com/proxy-server-list/country-vn/"), ProxynovaPage + "Vietnam", false, ProxynovaEx),
        };
    }
}
