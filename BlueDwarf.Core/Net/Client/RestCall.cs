// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Client
{
    using System.Reflection;
    using Reflection;
    using Utility;

    /// <summary>
    /// Represents a (very) simple REST call
    /// </summary>
    internal class RestCall
    {
        /// <summary>
        /// Gets the HTTP verb.
        /// </summary>
        /// <value>
        /// The verb.
        /// </value>
        public string Verb { get; private set; }
        /// <summary>
        /// Gets the URL path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestCall"/> class.
        /// </summary>
        /// <param name="verb">The verb.</param>
        /// <param name="path">The path.</param>
        private RestCall(string verb, string path)
        {
            Verb = verb;
            Path = path;
        }

        /// <summary>
        /// Creates a REST call from a given method.
        /// If the method has no marker, a null is returned
        /// </summary>
        /// <param name="methodBase">The method base.</param>
        /// <returns></returns>
        public static RestCall FromMethod(MethodBase methodBase)
        {
            var httpGet = methodBase.GetCustomAttribute<HttpGetAttribute>();
            if (httpGet != null)
                return new RestCall("GET", httpGet.UriTemplate);
            return null;
        }
    }
}