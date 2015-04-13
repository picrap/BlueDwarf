// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Utility
{
    using System;
    using System.Net;

    /// <summary>
    /// Extensions to <see cref="Uri"/>
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Gets the network credential from the given URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>Credential or null if none specified</returns>
        public static NetworkCredential GetNetworkCredential(this Uri uri)
        {
            var userPassword = uri.UserInfo;
            if (string.IsNullOrEmpty(userPassword))
                return null;
            var parts = userPassword.Split(new[] { ':' }, 2);
            if (parts.Length != 2)
                return null;
            return new NetworkCredential(parts[0], parts[1]);
        }

        /// <summary>
        /// Gets a literal representation of host and port from the given URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static string GetHostAndPort(this Uri uri)
        {
            return string.Format("{0}:{1}", uri.Host, uri.Port);
        }
    }
}
