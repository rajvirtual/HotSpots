using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Facebook;
using HotSpots.Facebook;
using Microsoft.Phone.Controls;

namespace HotSpots
{
    public partial class FaceBookBrowser : PhoneApplicationPage
    {
        private FBWallPost m_fbPost;
        private WebClient m_wcPostMessage;
        public FaceBookBrowser()
        {
            InitializeComponent();
            DataContext = App.DataViewModel;
            Loaded += new RoutedEventHandler(FaceBookBrowser_Loaded);
        }

        void FaceBookBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            wbLogin.NavigateToString("javascript:void((function(){var a,b,c,e,f;f=0;a=document.cookie.split('; ');for(e=0;e<a.length&&a[e];e++){f++;for(b='.'+location.host;b;b=b.replace(/^(?:%5C.|[^%5C.]+)/,'')){for(c=location.pathname;c;c=c.replace(/.$/,'')){document.cookie=(a[e]+'; domain='+b+'; path='+c+'; expires='+new Date((new Date()).getTime()-1e11).toGMTString());}}}})())");
            wbLogin.Navigate(FBUris.GetLoginUri());
        }


        private void wbLogin_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (e.Uri == null){
                return;
            }
            string strLoweredAddress = e.Uri.OriginalString.ToLower();
            if (strLoweredAddress.StartsWith("https://login.facebook.com/login.php?app_id"))
            {
                txtError.Text = "Try to login again - this should help";
                //donno why (it is already true) but setting is script enabled enables helps here:)
                wbLogin.IsScriptEnabled = true;
                wbLogin.Navigate(FBUris.GetLoginUri());    //login again
                return;
            }
            if (strLoweredAddress.StartsWith("http://www.facebook.com/connect/login_success.html?code="))
            {
                txtStatus.Text = "Trying to retrieve access token";
                wbLogin.Navigate(FBUris.GetTokenLoadUri(e.Uri.OriginalString.Substring(56)));
                return;
            }
            string strTest = wbLogin.SaveToString();
            if (strTest.Contains("access_token"))
            {
                int nPos = strTest.IndexOf("access_token");
                string strPart = strTest.Substring(nPos + 13);
                nPos = strPart.IndexOf("</PRE>");
                strPart = strPart.Substring(0, nPos);
                //REMOVE EXPIRES IF FOUND!
                nPos = strPart.IndexOf("&amp;expires=");
                if (nPos != -1)
                {
                    strPart = strPart.Substring(0, nPos);
                }
                App.AccessToken = strPart;
                //automaticall leave the page after login success
                m_fbPost = new FBWallPost(false);
                var viewModel = DataContext as ViewModel;
                m_fbPost.TheMessage = viewModel.PostMessageText;
                m_fbPost.TheName = viewModel.SelectedLocation.Name;
                //m_fbPost.ThePictureLink = viewModel.SelectedLocation.ImgSource;

                var link = viewModel.MoreInfos.Links.Where(w => w.Text == "Website").FirstOrDefault();
                if (link != null)
                {
                    m_fbPost.TheLink = link.Url;
                }

                if (m_wcPostMessage == null)
                {
                    m_wcPostMessage = new WebClient();
                    m_wcPostMessage.UploadStringCompleted += m_wcPostMessage_UploadStringCompleted;
                }
                try
                {
                    m_wcPostMessage.UploadStringAsync(FBUris.GetPostMessageUri(),
                        "POST", m_fbPost.GetPostParameters(App.AccessToken));
                }
                catch (Exception eX)
                {
                    UpdateUIStatus("Post to wall failed", eX.Message);
                }

                return;
            }
        }

        private void UpdateUIStatus(string strStatus, string strError)
        {
            txtStatus.Text = strStatus;
            txtError.Text = strError;
        }

        void m_wcPostMessage_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                UpdateUIStatus("Error posting message", e.Error.Message);
                return;
            }
            try
            {
                MessageBox.Show("Message posted to Facebook Wall");
             //   UpdateUIStatus("Post done", e.Result);
                NavigationService.GoBack();
            }
            catch (Exception eX)
            {
                UpdateUIStatus("Error handling post result", eX.Message);
            }
        }





    }
}