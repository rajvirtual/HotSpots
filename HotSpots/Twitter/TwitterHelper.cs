using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Windows;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;

namespace HotSpots.Twitter
{
    public class TwitterHelper
    {
        private readonly TwitterAccess _twitterSettings;
        private readonly bool _authorized;
        private readonly OAuthCredentials _credentials;
        private readonly RestClient _client;
        public event EventHandler TweetCompletedEvent;
        public event EventHandler LoadedCompleteEvent;
        public event EventHandler FavoriteCompletedEvent;
        public event EventHandler ErrorEvent;

        public TwitterHelper()
        {
            _twitterSettings = Helper.LoadSetting<TwitterAccess>(Constants.TwitterAccess);

            if (_twitterSettings == null || String.IsNullOrEmpty(_twitterSettings.AccessToken) ||
               String.IsNullOrEmpty(_twitterSettings.AccessTokenSecret))
            {
                return;
            }

            _authorized = true;

            _credentials = new OAuthCredentials
            {
                Type = OAuthType.ProtectedResource,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = TwitterSettings.ConsumerKey,
                ConsumerSecret = TwitterSettings.ConsumerKeySecret,
                Token = _twitterSettings.AccessToken,
                TokenSecret = _twitterSettings.AccessTokenSecret,
                Version = TwitterSettings.OAuthVersion,
            };

            _client = new RestClient
            {
                Authority = "http://api.twitter.com",
                HasElevatedPermissions = true
            };
        }

        public void NewTweet(string tweetText)
        {
            if (!_authorized)
            {
                if (ErrorEvent != null)
                    ErrorEvent(this, EventArgs.Empty);
                return;
            }

            var request = new RestRequest
            {
                Credentials = _credentials,
                Path = "/statuses/update.xml",
                Method = WebMethod.Post
            };

            request.AddParameter("status", tweetText);

            _client.BeginRequest(request, new RestCallback(NewTweetCompleted));
        }

        private void NewTweetCompleted(RestRequest request, RestResponse response, object userstate)
        {
            // We want to ensure we are running on our thread UI
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (TweetCompletedEvent != null)
                            TweetCompletedEvent(this, EventArgs.Empty);
                    }
                    else
                    {
                        if (ErrorEvent != null)
                            ErrorEvent(this, EventArgs.Empty);
                    }
                });
        }



    }
}
