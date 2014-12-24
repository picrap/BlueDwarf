using System;

namespace BlueDwarf.Net.Proxy.Client
{
    public class ProxyClientConnectEventArgs : EventArgs
    {
        public string TargetHost { get; private set; }
        public int TargetPort { get; private set; }

        public ProxyClientConnectEventArgs(string targetHost, int targetPort)
        {
            TargetHost = targetHost;
            TargetPort = targetPort;
        }
    }
}
