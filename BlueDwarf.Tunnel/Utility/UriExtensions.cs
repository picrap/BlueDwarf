// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Utility
{
    using System;
    using System.Net;

    public static class UriExtensions
    {
        public static NetworkCredential GetNetworkCredential(this Uri uri)
        {
            var userPassword = uri.UserInfo;
            if (string.IsNullOrEmpty(userPassword))
                return null;
            var parts = userPassword.Split(new[] { ':' }, 2);
            return new NetworkCredential(parts[0], parts[1]);
        }

        public static string GetHostAndPort(this Uri uri)
        {
            return string.Format("{0}:{1}", uri.Host, uri.Port);
        }
    }
}
