// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Name.StatDns
{
    using System.Runtime.Serialization;

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
}