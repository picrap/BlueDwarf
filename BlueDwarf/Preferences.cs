
using System;
using System.Runtime.Serialization;

namespace BlueDwarf
{
    [DataContract]
    public class Preferences
    {
        public const string SocksListeningPortKey = "SocksListeningPort";
        [DataMember(Name = SocksListeningPortKey)]
        public int SocksListeningPort { get; set; }

        public const string LocalProxyKey = "Proxy1";
        [DataMember(Name = LocalProxyKey)]
        public Uri LocalProxy { get; set; }

        public const string RemoteProxyKey = "Proxy2";
        [DataMember(Name = RemoteProxyKey)]
        public Uri RemoteProxy { get; set; }

        public const string TestTargetKey = "ProxyTest";
        [DataMember(Name = TestTargetKey)]
        public Uri TestTarget { get; set; }

        public const string KeepAlive1Key = "KeepAlive1";
        [DataMember(Name = KeepAlive1Key)]
        public Uri KeepAlive1 { get; set; }

        public const string KeepAlive1IntervalKey = "KeepAlive1Internal";
        [DataMember(Name = KeepAlive1IntervalKey)]
        public int KeepAlive1Interval { get; set; }

        public const string KeepAlive2Key = "KeepAlive2";
        [DataMember(Name = KeepAlive2Key)]
        public Uri KeepAlive2 { get; set; }

        public const string KeepAlive2IntervalKey = "KeepAlive2Internal";
        [DataMember(Name = KeepAlive2IntervalKey)]
        public int KeepAlive2Interval { get; set; }

        public Preferences()
        {
            TestTarget = new Uri("https://google.com");
            KeepAlive1 = new Uri("https://google.com");
            KeepAlive1Interval = 120;
            KeepAlive2Interval = 120;
            SocksListeningPort = 3128;
        }
    }
}
