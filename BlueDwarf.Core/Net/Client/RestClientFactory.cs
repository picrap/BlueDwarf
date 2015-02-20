// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Client
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Remoting.Proxies;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using Http;
    using Proxy.Client;
    using Utility;

    public class RestClientFactory
    {
        public TClient CreateClient<TClient>(Uri hostAddress, ProxyRoute proxyRoute)
        {
            var proxy = new RestClientProxy(typeof(TClient), hostAddress, proxyRoute);
            return (TClient)proxy.GetTransparentProxy();
        }
    }

    internal class RestCall
    {
        public string Verb { get; private set; }
        public string Path { get; private set; }

        private RestCall(string verb, string path)
        {
            Verb = verb;
            Path = path;
        }

        public static RestCall FromMethod(MethodBase methodBase)
        {
            var httpGet = methodBase.GetCustomAttribute<HttpGetAttribute>();
            if (httpGet != null)
                return new RestCall("GET", httpGet.UriTemplate);
            return null;
        }
    }

    internal class RestClientProxy : RealProxy
    {
        private readonly Uri _hostAddress;
        private readonly ProxyRoute _proxyRoute;

        public RestClientProxy(Type interfaceType, Uri hostAddress, ProxyRoute proxyRoute)
            : base(interfaceType)
        {
            _hostAddress = hostAddress;
            _proxyRoute = proxyRoute;
        }

        public override IMessage Invoke(IMessage message)
        {
            var methodCallMessage = message as IMethodCallMessage;
            if (methodCallMessage != null)
                return InvokeMethod(methodCallMessage);

            return message;
        }

        private IMessage InvokeMethod(IMethodCallMessage methodCallMessage)
        {
            var invokedMethod = (MethodInfo)methodCallMessage.MethodBase;
            var restCall = RestCall.FromMethod(invokedMethod);
            if (restCall != null)
            {
                var path = "/" + restCall.Path.TrimStart('/');
                var parameters = invokedMethod.GetParameters();
                for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
                {
                    var parameter = methodCallMessage.Args[parameterIndex];
                    var literalParameter = parameter != null ? parameter.ToString() : "";
                    var literalParameterPlaceholder = "{" + parameters[parameterIndex].Name + "}";
                    path = path.Replace(literalParameterPlaceholder, literalParameter);
                }

                using (var stream = _proxyRoute.Connect(_hostAddress.Host, _hostAddress.Port, false))
                {
                    var request = new HttpRequest(restCall.Verb, path).AddHeader("Host", _hostAddress.GetHostAndPort()).AddHeader("Connection", "Close").AddHeader("Proxy-Connection", "Keep-Alive");
                    request.Write(stream);
                    var response = HttpResponse.FromStream(stream);
                    if (response.StatusCode >= 400)
                    {
                        var errorText = response.ReadContentString(stream);
                        throw new InvalidOperationException(errorText);
                    }
                    if (invokedMethod.ReturnType != typeof(void))
                    {
                        var contentString = response.ReadContentString(stream);
                        using (var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(contentString)))
                        {
                            var serializer = new DataContractJsonSerializer(invokedMethod.ReturnType);
                            var result = serializer.ReadObject(contentStream);
                            return new ReturnMessage(result, new object[0], 0, methodCallMessage.LogicalCallContext, methodCallMessage);
                        }
                    }
                }
            }
            return new ReturnMessage(null, new object[0], 0, methodCallMessage.LogicalCallContext, methodCallMessage);
        }
    }
}
