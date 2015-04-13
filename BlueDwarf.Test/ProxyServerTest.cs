// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Net.Proxy;

    [TestClass]
    public class ProxyServerTest
    {
        [TestMethod]
        [TestCategory("ProxyServer")]
        public void LiteralWindowsProxyServerTest()
        {
            var p = new ProxyServer("http=1.2.3.4:8080");
            Assert.AreEqual(ProxyProtocol.HttpConnect, p.Protocol);
            Assert.AreEqual("1.2.3.4", p.Host);
            Assert.AreEqual(8080, p.Port);
        }

        [TestMethod]
        [TestCategory("ProxyServer")]
        public void LiteralUriProxyServerTest()
        {
            var p = new ProxyServer("socks://[::1]:1080");
            Assert.AreEqual(ProxyProtocol.Socks4A, p.Protocol);
            // WTF? there is a full expansion of IPv6 addresses in URI parsing.
            Assert.AreEqual("[0000:0000:0000:0000:0000:0000:0000:0001]", p.Host);
            Assert.AreEqual(1080, p.Port);
        }
    }
}
