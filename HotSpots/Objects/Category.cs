using System;
using System.Runtime.Serialization;

namespace HotSpots.Objects
{
    [DataContract]
    public class Category
    {
        [DataMember(Name="name")]
        public String Name
        {
            get;
            set;
        }

        [DataMember(Name="category_filter")]
        public String Filter
        {
            get;
            set;
        }

        [DataMember(Name="search_url")]
        public Uri Url
        {
            get;
            set;
        }
    }
}
