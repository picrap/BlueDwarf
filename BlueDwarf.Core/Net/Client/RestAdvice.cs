// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Client
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using ArxOne.MrAdvice.Advice;
    using Http;
    using Proxy.Client;
    using Utility;

    /// <summary>
    /// REST call handler
    /// Currently does not handle POST with body
    /// Also only supports JSON messages
    /// </summary>
    internal class RestAdvice : Attribute, IMethodAdvice
    {
        private readonly Uri _hostAddress;
        private readonly Route _route;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestAdvice"/> class.
        /// </summary>
        /// <param name="hostAddress">The host address.</param>
        /// <param name="Route">The proxy route.</param>
        public RestAdvice(Uri hostAddress, Route route)
        {
            _hostAddress = hostAddress;
            _route = route;
        }

        /// <summary>
        /// Implements advice logic.
        /// Usually, advice must invoke context.Proceed()
        /// </summary>
        /// <param name="context">The method advice context.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public void Advise(MethodAdviceContext context)
        {
            var invokedMethod = (MethodInfo)context.TargetMethod;
            var restCall = RestCall.FromMethod(invokedMethod);
            if (restCall == null)
                throw new NotImplementedException();

            // first, replace the parameters
            // (not elegant way)
            var path = restCall.Path;
            var parameters = invokedMethod.GetParameters();
            for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
            {
                var parameter = context.Parameters[parameterIndex];
                var literalParameter = parameter != null ? parameter.ToString() : "";
                var literalParameterPlaceholder = "{" + parameters[parameterIndex].Name + "}";
                path = path.Replace(literalParameterPlaceholder, literalParameter);
            }

            // then create the route and send the request
            using (var stream = _route.Connect(_hostAddress.Host, _hostAddress.Port, true))
            {
                var request = new HttpRequest(restCall.Verb, path)
                    .AddHeader("Host", _hostAddress.GetHostAndPort())
                    .AddHeader("Connection", "Close")
                    .AddHeader("Proxy-Connection", "Keep-Alive");
                request.Write(stream);
                var response = HttpResponse.FromStream(stream);
                if (response.StatusCode >= 400)
                {
                    var errorText = response.ReadContentString(stream);
                    throw new InvalidOperationException(errorText);
                }
                // handle return value
                if (invokedMethod.ReturnType != typeof(void))
                {
                    var contentString = response.ReadContentString(stream);
                    using (var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(contentString)))
                    {
                        var serializer = new DataContractJsonSerializer(invokedMethod.ReturnType);
                        var result = serializer.ReadObject(contentStream);
                        context.ReturnValue = result;
                    }
                }
            }
        }
    }
}