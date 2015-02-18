using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace HotSpots
{
    public partial class Browser : PhoneApplicationPage
    {
        public Browser()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            string url;
            NavigationContext.QueryString.TryGetValue("url", out url);
            webBrowser1.Navigate(new Uri(url, UriKind.RelativeOrAbsolute));

        }
    }
}