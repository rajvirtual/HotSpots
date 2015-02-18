using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HotSpots.Objects
{
    [DataContract]
    public class SearchResult
    {
        [DataMember(Name="businesses")]
        public List<Business> Businesses
        {
            get;
            set;
        }

        [DataMember(Name = "message")]
        public Message Message
        {
            get;
            set;
        }
    }
}
