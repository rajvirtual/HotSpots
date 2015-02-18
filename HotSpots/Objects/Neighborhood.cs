using System;
using System.Runtime.Serialization;

namespace HotSpots.Objects
{
    [DataContract]
    public class Neighborhood
    {
        [DataMember(Name="name")]
        public String Name
        {
            get;
            set;
        }

        [DataMember(Name="url")]
        public Uri Url
        {
            get;
            set;
        }
    }
}
