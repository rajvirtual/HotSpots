using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HotSpots.Twitter;
using Microsoft.Advertising.Mobile.UI;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace HotSpots
{
    public partial class Details : PhoneApplicationPage
    {
        private readonly ViewModel _viewModel;
        private Popup _popup;

        public Details()
        {
            InitializeComponent();
            _viewModel = App.DataViewModel;
            DataContext = App.DataViewModel;
            DetailsPivot.Visibility = _viewModel.USLocation ? Visibility.Visible : Visibility.Collapsed;
            DetailsNonUsPivot.Visibility = _viewModel.USLocation ? Visibility.Collapsed : Visibility.Visible;
            if (!_viewModel.USLocation)
            {
                listboxNonUs.SelectedIndex = -1;
                _viewModel.SetNonUSDetail();
            }
            Loaded += Details_Loaded;
        }

        void Details_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = App.DataViewModel;
            if (!viewModel.CurrentLat.IsNullOrEmpty())
            {
                //if (adDetails.Location.Latitude <= 0)
                //{
                //    adDetails.Location = new Location(viewModel.CurrentLat.ToDouble(), viewModel.CurrentLon.ToDouble());
                //}
            }

            _viewModel.Busy = false;
            _viewModel.Loaded = true;
            if (!_viewModel.USLocation)
            {
            }
            else
            {
                CallImpressionTrackerForProfile();
            }
        }

        private void CallImpressionTrackerForProfile()
        {
            var viewModel = App.DataViewModel;
            try
            {
                var html = UserResources.ImpressionTracker.Replace("{listing_profile}", "listing_profile").
                    Replace("{listing_id}", viewModel.SelectedLocation.LocationId).
                    Replace("{publisher}", App.Publisher).
                    Replace("{reference_id}", viewModel.MoreInfos.ReferenceId).
                    Replace("{muid}", App.DeviceId);
                browser.NavigateToString(html);

            }
            catch (Exception)
            {


            }
        }

        private void CallImpressionTrackerForReview()
        {
            var viewModel = App.DataViewModel;
            if (viewModel.MoreInfos == null)
            {
                return;
            }
            try
            {
                var html = UserResources.ImpressionTracker.Replace("{listing_profile}", "listing_review").
                            Replace("{listing_id}", viewModel.SelectedLocation.LocationId).
                            Replace("{publisher}", App.Publisher).
                            Replace("{reference_id}", viewModel.MoreInfos.ReferenceId).
                            Replace("{muid}", App.DeviceId);
                browser.NavigateToString(html);

            }
            catch (Exception)
            {

            }
        }

        protected override void OnBackKeyPress
            (System.ComponentModel.CancelEventArgs e)

        {
            if ((_popup != null) && (_popup.IsOpen))
            {
                _popup.IsOpen = false;
                e.Cancel = true;
            }
            else
            {
                base.OnBackKeyPress(e);
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (_popup != null)
            {
                _popup.IsOpen = false;
            }
            Utils.StopShakeDetector();
            base.OnNavigatedFrom(e);
        }



        private void ImageSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImageList.SelectedItem != null)
            {
                var url = "/Gallery.Xaml?imageUrl=" + ImageList.SelectedItem;
                detailPage.NavigationService.Navigate(new Uri(url, UriKind.Relative));
            }
        }


        private void ReviewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = _viewModel.SelectedReview;
            if (selectedItem != null)
            {
                var url = "/Browser.Xaml?url=" + selectedItem.CitySearchUrl;
                detailPage.NavigationService.Navigate(new Uri(url, UriKind.Relative));

            }
        }

        private void OffersClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/OffersPage.Xaml", UriKind.Relative));

        }

        private void AddressClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = _viewModel.SelectedLocation;
            if (!String.IsNullOrEmpty(selectedItem.Phone))
            {
                var url = "/MapPage.Xaml";
                detailPage.NavigationService.Navigate(new Uri(url, UriKind.Relative));
            }
        }

        private void PhoneClick(object sender, RoutedEventArgs e)
        {
            var vc = VibrateController.Default;
            vc.Start(TimeSpan.FromMilliseconds(100));
            var selectedItem = _viewModel.SelectedLocation;
            if (!String.IsNullOrEmpty(selectedItem.Phone))
            {
                var task = new PhoneCallTask();
                task.PhoneNumber = selectedItem.Phone;
                task.Show();
            }

        }

        private void AboutClick(object sender, GestureEventArgs e)
        {
            var selectedItem = _viewModel.MoreInfos;
            if (selectedItem != null)
            {
                var url = "/Browser.Xaml?url=" + selectedItem.CustomerMessageUrl;
                NavigationService.Navigate(new Uri(url, UriKind.Relative));

            }
        }

        private void Detail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DetailsPivot.SelectedIndex == 2)
            {
                CallImpressionTrackerForReview();
            }
        }

        private void ShowPopup()
        {
            this._popup = new Popup();
            var child = new PostMessageWindow();
            child.btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
            child.btnOk.Click += new RoutedEventHandler(btnOk_Click);
            child.VerticalAlignment = VerticalAlignment.Center;
            this._popup.Child = child;
            this._popup.IsOpen = true;
        }

        void btnOk_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.PostMessageText = (_popup.Child as PostMessageWindow).postMessageText.Text;
            var url = "/FaceBookBrowser.xaml";
            NavigationService.Navigate(new Uri(url, UriKind.Relative));
            _popup.IsOpen = false;
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _popup.IsOpen = false;
        }

        private void TwitterClick()
        {
            var credentialsFound = false;
            var twitterSettings = Helper.LoadSetting<TwitterAccess>(Constants.TwitterAccess);
            if ((twitterSettings != null &&
                !String.IsNullOrEmpty(twitterSettings.AccessToken) &&
                !String.IsNullOrEmpty(twitterSettings.AccessTokenSecret)))
            {
                credentialsFound = true;
            }

            Dispatcher.BeginInvoke(delegate
            {
                NavigationService.Navigate(credentialsFound
                                        ? new Uri("/TweetPage.xaml", UriKind.Relative)
                                        : new Uri("/TwitterAuthPage.xaml", UriKind.Relative));
            });
        }

        private void LinkClick(object sender, RoutedEventArgs e)
        {
            var linkData = (sender as Button).DataContext as LinksData;
            if (!string.IsNullOrEmpty(linkData.Url))
            {
                if (linkData.Url.Contains("Phone"))
                {
                    PhoneClick(null, null);

                }
                else if (linkData.Url.Contains("Map"))
                {
                    AddressClick(null, null);
                }
                else if (linkData.Url.Contains("Facebook"))
                {
                    _viewModel.PostMessageCaption = "Post a message to your Facebook wall.";
                    _viewModel.PostMessageMaxLength = 420;
                    ShowPopup();
                }
                else if (linkData.Url.Contains("Twitter"))
                {
                    TwitterClick();
                }
                else
                {
                    var url = "/Browser.Xaml?url=" +  Utils.BuildReferenceUrl(linkData.Url);
                    NavigationService.Navigate(new Uri(url, UriKind.RelativeOrAbsolute));
                }
            }
        }

        private void NonUsReviewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = _viewModel.SelectedReview;
            if (selectedItem != null)
            {
                var url = "/Browser.Xaml?url=" + selectedItem.CitySearchUrl;
                detailPage.NavigationService.Navigate(new Uri(url, UriKind.Relative));

            }

        }

        private void EditorialSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = _viewModel.SelectedEditorial;
            if (selectedItem != null)
            {
                var url = "/Browser.Xaml?url=" + selectedItem.CitySearchUrl;
                detailPage.NavigationService.Navigate(new Uri(url, UriKind.Relative));

            }
        }
    }
}