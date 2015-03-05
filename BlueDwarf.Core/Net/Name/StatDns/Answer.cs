// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Name.StatDns
{
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("{Name} {TTL} {Class} {Type} {RData}")]
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
}