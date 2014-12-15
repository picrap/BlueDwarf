
using System;
using System.Net;

namespace BlueDwarf.Utility
{
    public static class UriExtensions
    {
        public static NetworkCredential GetNetworkCredential(this Uri uri)
        {
            var userPassword = uri.UserInfo;
            if (userPassword.IsNullOrEmpty())
                return null;
            var parts = userPassword.Split(new[] { ':' }, 2);
            return new NetworkCredential(parts[0], parts[1]);
        }
    }
}
