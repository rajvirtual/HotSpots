using System;
using System.Runtime.Serialization;

namespace HotSpots.Objects
{
    [DataContract]
    public class Message
    {
        [DataMember(Name = "code")]
        public ResponseCodes Code
        {
            get;
            set;
        }

        [DataMember(Name = "text")]
        public String Text
        {
            get;
            set;
        }

        [DataMember(Name = "version")]
        public String Version
        {
            get;
            set;
        }
    }
}
