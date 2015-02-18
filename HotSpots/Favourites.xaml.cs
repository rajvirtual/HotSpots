using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace HotSpots
{
    public partial class Favourites : PhoneApplicationPage
    {
        private Popup _popup;
        public Favourites()
        {
            InitializeComponent();
            DataContext = App.DataViewModel;

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            App.DataViewModel.SelectedFavourite = null;
            base.OnNavigatedTo(e);
        }

        private void ShowPopup()
        {
            this._popup = new Popup();
            var child = new AddFave();
            child.btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
            child.btnOk.Click += new RoutedEventHandler(btnOk_Click);
            child.VerticalAlignment = VerticalAlignment.Center;
            this._popup.Child = child;
            this._popup.IsOpen = true;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = App.DataViewModel;
            var faveText = (_popup.Child as AddFave).faveText.Text;
            viewModel.AddFavourite(faveText);
            _popup.IsOpen = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _popup.IsOpen = false;
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ShowPopup();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            viewModel.DeleteFavourites();

        }

        private void Favourites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            var selectedItem = viewModel.SelectedFavourite;
            var searchCriteria = new SearchCriteria();
            if (selectedItem != null)
            {
                var bw = new BackgroundWorker();
                bw.DoWork += (s, args) =>
                {
                    args.Result = Utils.GetCurrentLocation();
                };
                GeoCoordinate coordinates = null;
                bw.RunWorkerCompleted += (s, args) =>
                {

                    coordinates = args.Result == null ? null : args.Result as GeoCoordinate;
                    if (coordinates == null)
                    {
                        return;
                    }
                    searchCriteria.LocationLat = coordinates.Latitude.ToString("0.0000");
                    searchCriteria.LocationLong = coordinates.Longitude.ToString("0.0000");

                    searchCriteria.SearchText = selectedItem.Code;
                    searchCriteria.NonUSSearchText = selectedItem.Code;
                    App.DataViewModel.ProgressBarText = "Searching " + searchCriteria.SearchText + " nearby...";


                    searchCriteria.CriteriaType = SearchCriteriaType.What;
                    Utils.SearchListResults(searchCriteria);
                    const string url = "/ListResults.Xaml";
                    try
                    {
                        NavigationService.Navigate(new Uri(url, UriKind.Relative));
                    }
                    catch (Exception)
                    {

                    }

                };

                bw.RunWorkerAsync();
            }
        }
    }
}