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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace HotSpots
{
    public partial class Gallery : PhoneApplicationPage
    {
        public Gallery()
        {
            InitializeComponent();
            DataContext = App.DataViewModel;
            Loaded += delegate
            {
                string imageUrl;
                NavigationContext.QueryString.TryGetValue("imageUrl", out imageUrl);
                SelectedImage.Source = new BitmapImage(new Uri(imageUrl, UriKind.RelativeOrAbsolute));
            };

        }

        private void DismissGallery(object sender, GestureEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}