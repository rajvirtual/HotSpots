using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Devices;
using Microsoft.Phone.Controls;


namespace HotSpots
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher _watcher = null;
        private readonly ViewModel _viewModel;
        // Constructor

        public MainPage()
        {
            InitializeComponent();
            Loaded += delegate
            {
                App.CurrentNavigationService = NavigationService; 
                _viewModel.Loaded = true;
                _viewModel.Busy = false;
            };
            DataContext = App.DataViewModel;
            _viewModel = App.DataViewModel;
            _viewModel.AllowedServiceLocator = false;
            _viewModel.Loaded = true;
            _viewModel.Busy = false;
            if (Microsoft.Devices.Environment.DeviceType == DeviceType.Emulator)
            {
                return;
            }

            if (!Utils.HasAllowedLocationServices)
            {
                Utils.ShowLocationServicesAllowWarning();
            }

        }

        private void TapSearch(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Search.xaml", UriKind.RelativeOrAbsolute));
        }


        private void DoCategoryClicked(GeoCoordinate coordinates, MenuCategory category)
        {
            if (coordinates == null)
            {
                return;
            }
            _viewModel.CurrentLat = coordinates.Latitude.ToString("0.0000");
            _viewModel.CurrentLon = coordinates.Longitude.ToString("0.0000");
            if (_viewModel.SelectedMenuCategory.Code == "favourite")
            {
                _viewModel.LoadFavourites();
                NavigationService.Navigate(new Uri("/Favourites.xaml", UriKind.RelativeOrAbsolute));
                return;
            }

            var searchCriteria = new SearchCriteria();

            searchCriteria.LocationLat = _viewModel.CurrentLat;
            searchCriteria.LocationLong = _viewModel.CurrentLon;
            searchCriteria.LocationType = SearchLocationType.Local;

            searchCriteria.SearchText = category.Code;
            searchCriteria.NonUSSearchText = category.NonUSCode;
            searchCriteria.CriteriaType = category.CriteriaType;
            searchCriteria.NonUSCriteriaType = category.NonUSCriteriaType;
            _viewModel.ProgressBarText = "Searching " + category.Name + " nearby...";
            Utils.SearchListResults(searchCriteria);
            const string url = "/ListResults.Xaml";
            try
            {
                NavigationService.Navigate(new Uri(url, UriKind.Relative));
            }
            catch (Exception)
            {
            }
        }


        private void CategoryClicked(object sender, RoutedEventArgs e)
        {
            var category = (sender as Button).DataContext as MenuCategory;
            var bw = new BackgroundWorker();
            bw.DoWork += (s, args) =>
            {
                args.Result = Utils.GetCurrentLocation();

            };
            _viewModel.SelectedMenuCategory = category;
            bw.RunWorkerCompleted += (s, args) =>
            {
                DoCategoryClicked(args.Result == null ? null : args.Result as GeoCoordinate, category);
            };

            bw.RunWorkerAsync();

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (_watcher != null)
            {
                _watcher.Stop();
            }
            base.OnNavigatedTo(e);
        }

    }
}