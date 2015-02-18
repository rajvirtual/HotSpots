using System.Net;

namespace HotSpots.Facebook {
	public class FBWallPost {
		public string TheCaption { get; set; }
		public string TheDescription { get; set; }
		public string TheLink { get; set; }
		public string TheMessage { get; set; }
		public string TheName { get; set; }
		public string ThePictureLink { get; set; }

		public FBWallPost() { }
		public FBWallPost(bool bFillDefaults) {
			if(bFillDefaults) {
				TheCaption = "Testcaption";
				TheDescription = "Testdescription";
				TheLink = "http://iphone.pp-p.net";
				TheMessage = "Testmessage sent from windows phone 7";
				TheName = "Testname";
				ThePictureLink = "http://games.pp-p.net/Badges/RedAE.png";
			}

		}


		public string GetPostParameters(string strAccessToken) {
			try {
				string strRet = "access_token=" + strAccessToken;
				if(!string.IsNullOrEmpty(TheCaption)) {
					strRet += "&caption=" + HttpUtility.UrlEncode(TheCaption);
				}
				if(!string.IsNullOrEmpty(TheDescription)) {
					strRet += "&description=" + HttpUtility.UrlEncode(TheDescription);
				}
				if(!string.IsNullOrEmpty(TheLink)) {
					strRet += "&link=" + HttpUtility.UrlEncode(TheLink);
				}
				if(!string.IsNullOrEmpty(TheMessage)) {
					strRet += "&message=" + HttpUtility.UrlEncode(TheMessage);
				}
				if(!string.IsNullOrEmpty(TheName)) {
					strRet += "&name=" + HttpUtility.UrlEncode(TheName);
				}
				if(!string.IsNullOrEmpty(ThePictureLink)) {
					strRet += "&picture=" + HttpUtility.UrlEncode(ThePictureLink);
				}
				return (strRet);
			}
			catch { return (""); }
		}
	}
}
