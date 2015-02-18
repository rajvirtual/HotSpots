using System;
using System.Runtime.Serialization;

namespace HotSpots.Objects
{
    [DataContract]
    public class Review
    {
        [DataMember(Name="id")]
        public String Id
        {
            get;
            set;
        }

        [DataMember(Name="rating")]
        public Int32 Rating
        {
            get;
            set;
        }

        [DataMember(Name="rating_img_url")]
        public Uri RatingImageUrl
        {
            get;
            set;
        }

        [DataMember(Name="rating_img_url_small")]
        public Uri RatingImageUrlSmall
        {
            get;
            set;
        }

        [DataMember(Name="text_excerpt")]
        public String Excerpt
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

        [DataMember(Name="mobile_uri")]
        public Uri MobileUrl
        {
            get;
            set;
        }

        [DataMember(Name="user_name")]
        public String UserName
        {
            get;
            set;
        }

        [DataMember(Name="user_photo_url")]
        public Uri UserPhotoUrl
        {
            get;
            set;
        }

        [DataMember(Name="user_photo_url_small")]
        public Uri UserPhotoUrlSmall
        {
            get;
            set;
        }

        [DataMember(Name="user_url")]
        public Uri UserUrl
        {
            get;
            set;
        }

        [DataMember(Name = "date")]
        public string Date
        {
            get;
            set;
        }
    }
}
