// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client.Diagnostic
{
    using System;

    public class ProxyDiagnostic
    {
        /// <summary>
        /// If the system uses a proxy, it is set here
        /// </summary>
        /// <value>
        /// The default proxy.
        /// </value>
        public Uri DefaultProxy { get; set; }
        /// <summary>
        /// The route diagnostic to a safe (==commonly allowed) site by HTTP
        /// </summary>
        /// <value>
        /// The safe HTTP get route.
        /// </value>
        public RouteStatus SafeHttpGetRoute { get; set; }
        /// <summary>
        /// The route diagnostic to a safe (==commonly allowed) site by HTTPS
        /// </summary>
        /// <value>
        /// The safe HTTPS connect route.
        /// </value>
        public RouteStatus SafeHttpsConnectRoute { get; set; }
        /// <summary>
        /// The route diagnostic to a safe (==commonly allowed) site by HTTP, but with a HTTP CONNECT verb
        /// </summary>
        /// <value>
        /// The safe HTTP connect route.
        /// </value>
        public RouteStatus SafeHttpConnectRoute { get; set; }
        /// <summary>
        /// The route diagnostic to an unsafe (==commonly denied) site by HTTP
        /// </summary>
        /// <value>
        /// The safe HTTP get route.
        /// </value>
        public RouteStatus SensitiveHttpGetRoute { get; set; }
        /// <summary>
        /// The route diagnostic to an unsafe (==commonly denied) site by HTTPS
        /// </summary>
        /// <value>
        /// The sensitive HTTPS connect route.
        /// </value>
        public RouteStatus SensitiveHttpsConnectRoute { get; set; }
        /// <summary>
        /// The route diagnostic to an unsafe (==commonly denied) site by HTTP, but with a HTTP CONNECT verb
        /// </summary>
        /// <value>
        /// The sensitive HTTP connect route.
        /// </value>
        public RouteStatus SensitiveHttpConnectRoute { get; set; }
        /// <summary>
        /// Tells if the local DNS resolver works directly
        /// </summary>
        /// <value>
        ///   <c>true</c> if [safe local DNS]; otherwise, <c>false</c>.
        /// </value>
        public bool SafeLocalDns { get; set; }
        /// <summary>
        /// Tells if the local DNS resolver works for sensitive (==commonly denied) sites
        /// </summary>
        /// <value>
        ///   <c>true</c> if [sensitive local DNS]; otherwise, <c>false</c>.
        /// </value>
        public bool SensitiveLocalDns { get; set; }
    }
}
