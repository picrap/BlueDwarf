// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;

    /// <summary>
    /// Describes a web page, somehwere, which provides proxy servers lists
    /// </summary>
    public class ProxyPage
    {
        public string Name { get; private set; }
        public Uri PageUri { get; private set; }
        public bool ParseAsText { get; private set; }
        public string HostPortEx { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyPage"/> class.
        /// </summary>
        /// <param name="pageUri">The page URI.</param>
        /// <param name="name">The name.</param>
        /// <param name="parseAsText">if set to <c>true</c> [parse as text].</param>
        /// <param name="hostPortEx">The host port ex.</param>
        public ProxyPage(Uri pageUri, string name, bool parseAsText, string hostPortEx)
        {
            Name = name;
            PageUri = pageUri;
            ParseAsText = parseAsText;
            HostPortEx = hostPortEx;
        }

        /// <summary>
        /// Creates a Regex string to match a tag and its content.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="innerEx">The inner ex.</param>
        /// <returns></returns>
        private static string TagEx(string tag, string innerEx)
        {
            return string.Format(@"\<{0}[^>]*\>\s*({1})\s*\<\/{0}\>", tag.Replace(":", @"\:"), innerEx);
        }

        internal static readonly string ProxynovaEx = TagEx("span", HostPort.IPv4Ex) + ".*?" + TagEx("a", HostPort.PortEx);

        internal static readonly string XroxyRssHttpEx = TagEx("prx:ip", HostPort.IPv4Ex) + @"\s*" + TagEx("prx:port", HostPort.PortEx)
                                                     + @"\s*" + TagEx("prx:type", "Transparent|Anonymous") + @"\s*" + TagEx("prx:ssl", "true");
        internal static readonly string XroxyRssSocksEx = TagEx("prx:ip", HostPort.IPv4Ex) + @"\s*" + TagEx("prx:port", HostPort.PortEx)
                                                      + @"\s*" + TagEx("prx:type", "(?<protocol>(Socks|socks))(4|4A|4a|5)") + @"\s*" + TagEx("prx:ssl", "true");

        internal static readonly string XroxyRssEx = @"((" + XroxyRssHttpEx + @")|(" + XroxyRssSocksEx + @"))";

        private const string ProxynovaPage = "Proxynova: ";

        public static readonly ProxyPage[] Default = 
        {
            new ProxyPage(new Uri("http://www.xroxy.com/proxyrss.xml"), "Xroxy (RSS)", false, XroxyRssEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/"), "Proxynova (worldwide, recent checks)", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-ar/"), ProxynovaPage + "Argentina", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-au/"), ProxynovaPage + "Australia", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-at/"), ProxynovaPage + "Austria", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-br/"), ProxynovaPage + "Brazil", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-bd/"), ProxynovaPage + "Bangladesh", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-cl/"), ProxynovaPage + "Chile", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-cn/"), ProxynovaPage + "China", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-co/"), ProxynovaPage + "Colombia", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-fr/"), ProxynovaPage + "France", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-de/"), ProxynovaPage + "Germany", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-hk/"), ProxynovaPage + "Hong Kong", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-in/"), ProxynovaPage + "India", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-id/"), ProxynovaPage + "Indonesia", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-it/"), ProxynovaPage + "Italy", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-jp/"), ProxynovaPage + "Japan", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-my/"), ProxynovaPage + "Malaysia", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-ma/"), ProxynovaPage + "Morocco", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-pe/"), ProxynovaPage + "Peru", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-pl/"), ProxynovaPage + "Poland", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-pt/"), ProxynovaPage + "Portugal", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-ru/"), ProxynovaPage + "Russia", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-es/"), ProxynovaPage + "Spain", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-ch/"), ProxynovaPage + "Switzerland", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-tw/"), ProxynovaPage + "Taiwan", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-th/"), ProxynovaPage + "Thailand", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-ua/"), ProxynovaPage + "Ukraine", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-gb/"), ProxynovaPage + "United Kingdom", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-us/"), ProxynovaPage + "United States", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-ve/"), ProxynovaPage + "Venezuela", false, ProxynovaEx),
            new ProxyPage(new Uri("http://www.proxynova.com/proxy-server-list/country-vn/"), ProxynovaPage + "Vietnam", false, ProxynovaEx),
        };
    }
}
