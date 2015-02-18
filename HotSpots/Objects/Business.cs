using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HotSpots.Objects
{
    [DataContract]
    public class Business
    {
        [DataMember(Name="id")]
        public String Id
        {
            get;
            set;
        }

        [DataMember(Name="name")]
        public String Name
        {
            get;
            set;
        }

        [DataMember(Name="phone")]
        public String PhoneNumber
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

        #region Location

        [DataMember(Name="address1")]
        public String Address1
        {
            get;
            set;
        }

        [DataMember(Name="address2")]
        public String Address2
        {
            get;
            set;
        }

        [DataMember(Name="city")]
        public String City
        {
            get;
            set;
        }

        [DataMember(Name="state")]
        public String State
        {
            get;
            set;
        }

        [DataMember(Name="zip")]
        public String ZipCode
        {
            get;
            set;
        }

        [DataMember(Name="longitude")]
        public Double? Longitude
        {
            get;
            set;
        }

        [DataMember(Name="latitude")]
        public Double? Latitude
        {
            get;
            set;
        }

        [DataMember(Name="distance")]
        public Double? Distance
        {
            get;
            set;
        }

        #endregion

        #region Misc Urls

        [DataMember(Name="mobile_url")]
        public Uri MobileUrl
        {
            get;
            set;
        }

        [DataMember(Name="nearby_url")]
        public Uri NearbyUrl
        {
            get;
            set;
        }

        [DataMember(Name="photo_url")]
        public Uri PhotoUrl
        {
            get;
            set;
        }

        [DataMember(Name="photo_url_small")]
        public Uri PhotoUrlSmall
        {
            get;
            set;
        }

        #endregion

        #region Misc

        [DataMember(Name="is_closed")]
        public Boolean IsClosed
        {
            get;
            set;
        }

        [DataMember(Name="neighborhoods")]
        public List<Neighborhood> Neighborhoods
        {
            get;
            set;
        }

        [DataMember(Name="categories")]
        public List<Category> Categories
        {
            get;
            set;
        }

        #endregion

        #region Reviews and Ratings

        [DataMember(Name="avg_rating")]
        public Single AverageRating
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

        [DataMember(Name="review_count")]
        public Int32 ReviewCount
        {
            get;
            set;
        }

        [DataMember(Name="reviews")]
        public List<Review> Reviews
        {
            get;
            set;
        }

        #endregion
    }
}
