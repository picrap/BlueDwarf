#region Arx One
// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
#endregion

namespace BlueDwarf.Utility
{
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Extensions to <see cref="EndPoint"/>
    /// </summary>
    public static class EndPointExtensions
    {
        /// <summary>
        /// Convert an <see cref="EndPoint"/> to <see cref="IPEndPoint"/>.
        /// </summary>
        /// <param name="endPoint">The end point.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.IOException">When not possible</exception>
        public static IPEndPoint ToIPEndPoint(this EndPoint endPoint)
        {
            var ipEndPoint = endPoint as IPEndPoint;
            if (ipEndPoint != null)
                return ipEndPoint;
            var dnsEndPoint = endPoint as DnsEndPoint;
            if (dnsEndPoint != null)
            {
                var ipAddress = dnsEndPoint.TryResolveName();
                if (ipAddress != null)
                    return new IPEndPoint(ipAddress, dnsEndPoint.Port);
            }
            throw new IOException(string.Format("Can't handle type '{0}'", endPoint.GetType().FullName));
        }

        /// <summary>
        /// Tries to resolve the Name property from the given <see cref="DnsEndPoint"/>.
        /// </summary>
        /// <param name="dnsEndPoint">The DNS end point.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.IOException"></exception>
        public static IPAddress TryResolveName(this DnsEndPoint dnsEndPoint)
        {
            try
            {
                var addresses = Dns.GetHostAddresses(dnsEndPoint.Host);
                return addresses.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork)
                       ?? addresses.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetworkV6);
            }
            catch (SocketException e)
            {
                throw new IOException(string.Format("Can't resolve '{0}'", dnsEndPoint.Host), e);
            }
        }
    }
}
