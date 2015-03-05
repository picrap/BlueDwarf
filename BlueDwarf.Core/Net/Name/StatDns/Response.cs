// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Name.StatDns
{
    using System.Runtime.Serialization;

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
}