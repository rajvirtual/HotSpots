namespace HotSpots.Twitter
{
    public class TwitterSettings
    {
        // Make sure you set your own ConsumerKey and Secret from dev.twitter.com
        public static string ConsumerKey = "AHvrSh9GLo2BIPhyZbGLg";
        public static string ConsumerKeySecret = "dsrQCXwjL2AC9ISKdm8B486e5tcVefFCSqkbj97AWA";
        
        public static string RequestTokenUri = "https://api.twitter.com/oauth/request_token";
        public static string OAuthVersion = "1.0";
        public static string CallbackUri = "http://www.bing.com";
        public static string AuthorizeUri = "https://api.twitter.com/oauth/authorize";
        public static string AccessTokenUri = "https://api.twitter.com/oauth/access_token";
    }

    public class TwitterAccess
    {
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public string UserId { get; set; }
        public string ScreenName { get; set; }
    }
}
