// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Test
{
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Net.Proxy;
    using Net.Proxy.Scanner;

    [TestClass]
    public class HostScannerTest
    {
        [TestMethod]
        [TestCategory("Host Parsing")]
        public void ParsePortsTest()
        {
            var portRegex = new Regex(@"^" + HostPort.PortEx + HostScanner.AfterDigitEx);
            //var portRegex = new Regex(@"(?<port>(\d{1,4}))");
            for (int i = 0; i <= 65535; i++)
            {
                var match = portRegex.Match(i.ToString());
                Assert.IsTrue(match.Success);
                var p = int.Parse(match.Groups["port"].Value);
                Assert.AreEqual(i, p);
            }

            Assert.IsFalse(portRegex.Match("65536").Success);
            Assert.IsFalse(portRegex.Match("65540").Success);
            Assert.IsFalse(portRegex.Match("65600").Success);
            Assert.IsFalse(portRegex.Match("66000").Success);
            Assert.IsFalse(portRegex.Match("70000").Success);
        }

        [TestMethod]
        [TestCategory("Host Parsing")]
        public void ParseIP_0_0_0_0_Test()
        {
            var ipV4Regex = new Regex(@"^" + HostPort.IPv4Ex + HostScanner.AfterDigitEx);
            var match = ipV4Regex.Match("0.0.0.0");
            Assert.IsTrue(match.Success);
        }

        [TestMethod]
        [TestCategory("Host Parsing")]
        public void ParseIP_255_255_255_255_Test()
        {
            var ipV4Regex = new Regex(@"^" + HostPort.IPv4Ex + HostScanner.AfterDigitEx);
            var match = ipV4Regex.Match("255.255.255.255");
            Assert.IsTrue(match.Success);
        }

        [TestMethod]
        [TestCategory("Host Parsing")]
        public void ParseHostPort_123_45_67_89_7654_Test()
        {
            var ipV4Regex = new Regex(HostScanner.IPColonPortEx);
            var match = ipV4Regex.Match("123.45.67.89:7654");
            Assert.IsTrue(match.Success);
            Assert.AreEqual("123.45.67.89", match.Groups["address"].Value);
            Assert.AreEqual("7654", match.Groups["port"].Value);
        }

        [TestMethod]
        [TestCategory("Host Parsing")]
        public void ParsePageTest()
        {
            var hostScanner = new HostScanner();
            var hosts = hostScanner.Scan("here 123.45.67.89:3128 there 13.24.35.46 : 57 and everywhere 98.76.54.32  1").ToArray();
            Assert.IsTrue(hosts.Contains(new ProxyServer(ProxyProtocol.HttpConnect, IPAddress.Parse("123.45.67.89"), 3128)));
            Assert.IsTrue(hosts.Contains(new ProxyServer(ProxyProtocol.HttpConnect, IPAddress.Parse("13.24.35.46"), 57)));
            Assert.IsTrue(hosts.Contains(new ProxyServer(ProxyProtocol.HttpConnect, IPAddress.Parse("98.76.54.32"), 1)));
        }
    }
}
