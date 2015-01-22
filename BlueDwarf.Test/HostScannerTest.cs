using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlueDwarf.Test
{
    using System.Text.RegularExpressions;
    using Net.Proxy.Scanner;

    [TestClass]
    public class HostScannerTest
    {
        [TestMethod]
        public void ParsePortsTest()
        {
            var portRegex = new Regex(HostScanner.PortRangeEx);
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
    }
}
