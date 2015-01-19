// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Name
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using Annotations;
    using Http;
    using Proxy.Client;

    /// <summary>
    /// DNS resolution using the excellent statdns.com resolution
    /// </summary>
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    internal class StatDnsNameResolver : INameResolver
    {
        [DataContract]
        public class Question
        {
            [DataMember(Name = "name")]
            public string Name { get; set; }
            [DataMember(Name = "type")]
            public string Type { get; set; }
            [DataMember(Name = "class")]
            public string Class { get; set; }
        }

        [DataContract]
        public class Answer
        {
            [DataMember(Name = "name")]
            public string Name { get; set; }
            [DataMember(Name = "type")]
            public string Type { get; set; }
            [DataMember(Name = "class")]
            public string Class { get; set; }
            [DataMember(Name = "ttl")]
            public int TTL { get; set; }
            [DataMember(Name = "rdlength")]
            public int RDLength { get; set; }
            [DataMember(Name = "rdata")]
            public string RData { get; set; }
        }

        [DataContract]
        public class Response
        {
            [DataMember(Name = "question")]
            public Question[] Questions { get; set; }
            [DataMember(Name = "answer")]
            public Answer[] Answers { get; set; }
            [DataMember(Name = "authority")]
            public Answer[] Authorities { get; set; }
        }

        private readonly IDictionary<string, Tuple<IPAddress, DateTime>> _entries = new Dictionary<string, Tuple<IPAddress, DateTime>>();

        /// <summary>
        /// Resolves the specified name using the given route.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        public IPAddress Resolve(string name, ProxyRoute route)
        {
            IPAddress address;
            if (IPAddress.TryParse(name, out address))
                return address;

            var now = DateTime.UtcNow;
            lock (_entries)
            {
                Tuple<IPAddress, DateTime> entry;
                if (_entries.TryGetValue(name, out entry))
                {
                    if (entry.Item2 > now)
                        return entry.Item1;
                }

                var resolvedAddress = RequestionResolution(name, route);
                entry = Tuple.Create(resolvedAddress.Item1, now + TimeSpan.FromSeconds(resolvedAddress.Item2));
                _entries[name] = entry;
                return entry.Item1;
            }
        }

        private static Tuple<IPAddress, int> RequestionResolution(string name, ProxyRoute route)
        {
            for (int hop = 0; hop < 100; hop++)
            {
                var answer = Ask(name, "A", route) ?? Ask(name, "CNAME", route);
                if (answer == null)
                    return null;

                IPAddress address;
                if (IPAddress.TryParse(answer.RData, out address))
                    return Tuple.Create(address, answer.TTL);

                name = answer.RData;
            }
            return null;
        }

        /// <summary>
        /// Query to .
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private static Answer Ask(string name, string type, ProxyRoute route)
        {
            const string host = "api.statdns.com";
            using (var stream = route.Connect(host, 80, false))
            {
                new HttpRequest("GET", string.Format("/{0}/{1}", name, type.ToLower())).AddHeader("Host", host).Write(stream);
                var contentBytes = HttpResponse.FromStream(stream).ReadContent(stream);
                using (var memoryStream = new MemoryStream(contentBytes))
                {
                    var serializer = new DataContractJsonSerializer(typeof(Response));
                    var response = (Response)serializer.ReadObject(memoryStream);
                    if (response.Answers == null)
                        return null;
                    return response.Answers[0];
                }
            }
        }
    }
}