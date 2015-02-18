using System;

namespace HotSpots.Facebook {
	public static class FBUris {
		#region AppID
        private static string m_strAppID = "116123055132228";
		#endregion
		#region AppSecret - only needed because of the fragment bug
        private static string m_strAppSecret = "271174dc93a59b19e34b5010f4e21847";
		#endregion
		//the correct url - but not working due to the WebBrowser fragment bug
		//private static string m_strLoginURL = "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri=http://www.facebook.com/connect/login_success.html&type=user_agent&display=touch&scope=publish_stream,user_hometown";
		private static string m_strLoginURL = "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri=http://www.facebook.com/connect/login_success.html&display=touch&scope=publish_stream,user_hometown";
		private static string m_strGetAccessTokenURL = "https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri=http://www.facebook.com/connect/login_success.html&client_secret={1}&code={2}";
		private static string m_strQueryUserURL = "https://graph.facebook.com/me?fields=id,name,gender,link,hometown,picture&locale=en_US&access_token={0}";
		private static string m_strLoadFriendsURL = "https://graph.facebook.com/me/friends?access_token={0}";
		private static string m_strPostMessageURL = "https://graph.facebook.com/me/feed";

		public static Uri GetLoadFriendsUri(string strAccressToken) {
			return (new Uri(string.Format(m_strLoadFriendsURL, strAccressToken), UriKind.Absolute));
		}

		public static Uri GetPostMessageUri() {
			return (new Uri(m_strPostMessageURL, UriKind.Absolute));
		}
		public static Uri GetQueryUserUri(string strAccressToken) {
			return (new Uri(string.Format(m_strQueryUserURL, strAccressToken), UriKind.Absolute));
		}
		public static Uri GetLoginUri() {
			return (new Uri(string.Format(m_strLoginURL, m_strAppID), UriKind.Absolute));
		}

		public static Uri GetTokenLoadUri(string strCode) {
			return (new Uri(string.Format(m_strGetAccessTokenURL, m_strAppID, m_strAppSecret, strCode), UriKind.Absolute));
		}
	}
}