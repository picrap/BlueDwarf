namespace BlueDwarf.Net.Proxy
{
    using System;

    public interface IProxyConfiguration
    {
        void SetApplicationProxy(Uri proxy);
    }
}