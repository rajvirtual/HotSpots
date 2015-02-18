﻿using System;
using System.Windows;
using System.Windows.Navigation;
using Hammock;
using Hammock.Authentication.OAuth;
using HotSpots.Twitter;
using Microsoft.Phone.Controls;

namespace HotSpots
{
    public partial class TwitterAuthPage
    {
        private string _oAuthTokenSecret;
        private string _oAuthToken;

        public TwitterAuthPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetTwitterToken();
            ProgressBar.Visibility = Visibility.Visible;
            ProgressBar.IsIndeterminate = true;
        }

        private void GetTwitterToken()
        {
            var credentials = new OAuthCredentials
            {
                Type = OAuthType.RequestToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = TwitterSettings.ConsumerKey,
                ConsumerSecret = TwitterSettings.ConsumerKeySecret,
                Version = TwitterSettings.OAuthVersion,
                CallbackUrl = TwitterSettings.CallbackUri
            };

            var client = new RestClient
            {
                Authority = "https://api.twitter.com/oauth",
                Credentials = credentials,
                HasElevatedPermissions = true
            };

            var request = new RestRequest
            {
                Path = "/request_token"
            };
            client.BeginRequest(request, new RestCallback(TwitterRequestTokenCompleted));
        }

        private void TwitterRequestTokenCompleted(RestRequest request, RestResponse response, object userstate)
        {
            _oAuthToken = GetQueryParameter(response.Content, "oauth_token");
            _oAuthTokenSecret = GetQueryParameter(response.Content, "oauth_token_secret");
            var authorizeUrl = TwitterSettings.AuthorizeUri + "?oauth_token=" + _oAuthToken;

            if (String.IsNullOrEmpty(_oAuthToken) || String.IsNullOrEmpty(_oAuthTokenSecret))
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("error calling twitter"));
                return;
            }

            Dispatcher.BeginInvoke(() => BrowserControl.Navigate(new Uri(authorizeUrl)));
        }

        private static string GetQueryParameter(string input, string parameterName)
        {
            foreach (string item in input.Split('&'))
            {
                var parts = item.Split('=');
                if (parts[0] == parameterName)
                {
                    return parts[1];
                }
            }
            return String.Empty;
        }

        private void BrowserControl_Navigated(object sender, NavigationEventArgs e)
        {
            ProgressBar.IsIndeterminate = false;
            ProgressBar.Visibility = Visibility.Collapsed;
        }

        private void BrowserControl_Navigating(object sender, NavigatingEventArgs e)
        {
            ProgressBar.IsIndeterminate = true;
            ProgressBar.Visibility = Visibility.Visible;

            if (e.Uri.AbsoluteUri.CompareTo("https://api.twitter.com/oauth/authorize") == 0)
            {
                ProgressBar.IsIndeterminate = true;
                ProgressBar.Visibility = Visibility.Visible;
            }

            if (!e.Uri.AbsoluteUri.Contains(TwitterSettings.CallbackUri))
                return;

            e.Cancel = true;

            var arguments = e.Uri.AbsoluteUri.Split('?');
            if (arguments.Length < 1)
                return;

            GetAccessToken(arguments[1]);
        }

        private void GetAccessToken(string uri)
        {
            var requestToken = GetQueryParameter(uri, "oauth_token");
            if (requestToken != _oAuthToken)
            {
                MessageBox.Show("Twitter auth tokens don't match");
            }

            var requestVerifier = GetQueryParameter(uri, "oauth_verifier");

            var credentials = new OAuthCredentials
            {
                Type = OAuthType.AccessToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = TwitterSettings.ConsumerKey,
                ConsumerSecret = TwitterSettings.ConsumerKeySecret,
                Token = _oAuthToken,
                TokenSecret = _oAuthTokenSecret,
                Verifier = requestVerifier
            };

            var client = new RestClient
            {
                Authority = "https://api.twitter.com/oauth",
                Credentials = credentials,
                HasElevatedPermissions = true
            };

            var request = new RestRequest
            {
                Path = "/access_token"
            };

            client.BeginRequest(request, new RestCallback(RequestAccessTokenCompleted));
        }

        private void RequestAccessTokenCompleted(RestRequest request, RestResponse response, object userstate)
        {
            var twitteruser = new TwitterAccess
                                  {
                                      AccessToken = GetQueryParameter(response.Content, "oauth_token"),
                                      AccessTokenSecret = GetQueryParameter(response.Content, "oauth_token_secret"),
                                      UserId = GetQueryParameter(response.Content, "user_id"),
                                      ScreenName = GetQueryParameter(response.Content, "screen_name")
                                  };

            if (String.IsNullOrEmpty(twitteruser.AccessToken) || String.IsNullOrEmpty(twitteruser.AccessTokenSecret))
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show(response.Content));
                return;
            }

            Helper.SaveSetting(Constants.TwitterAccess, twitteruser);

            Dispatcher.BeginInvoke(() =>
                                       {
                                           {
                                               NavigationService.Navigate(new Uri("/TweetPage.xaml", UriKind.Relative));
                                           }
                                       });
        }
    }
}