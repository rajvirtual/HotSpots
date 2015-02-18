using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Advertising.Mobile.UI;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace HotSpots
{
    public partial class Search : PhoneApplicationPage
    {
        public Search()
        {
            InitializeComponent();
            DataContext = App.DataViewModel;
            Loaded += new RoutedEventHandler(Search_Loaded);
        }

        void Search_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = App.DataViewModel;
            if (!viewModel.CurrentLat.IsNullOrEmpty())
            {
                //if (adSearch.Location.Latitude <= 0)
                //{
                //    adSearch.Location = new Location(viewModel.CurrentLat.ToDouble(), viewModel.CurrentLon.ToDouble());
                //}
            }
        }




        private void SearchClick(object sender, RoutedEventArgs e)
        {
            var searchCriteria = new SearchCriteria();
            if (txtLocation.Text.ToLower() == "current location" || (txtLocation.Text.ToLower() == string.Empty))
            {
                DoCurrentSearch(searchCriteria);
                return;
            }
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
                App.DataViewModel.CurrentLat = coordinates.Latitude.ToString("0.0000");
                App.DataViewModel.CurrentLon = coordinates.Longitude.ToString("0.0000");
                searchCriteria.LocationType = SearchLocationType.Other;
                searchCriteria.SearchLocation = txtLocation.Text;
                if (string.IsNullOrEmpty(GetSearchTypeText()))
                {
                    searchCriteria.SearchText = App.DataViewModel.SelectedMenuCategory.Code;
                    searchCriteria.NonUSSearchText = App.DataViewModel.SelectedMenuCategory.NonUSCode;
                    App.DataViewModel.ProgressBarText = "Searching " + App.DataViewModel.SelectedMenuCategory.Name + " ...";
                }
                else
                {
                    searchCriteria.SearchText = GetSearchTypeText();
                    searchCriteria.NonUSSearchText = searchCriteria.SearchText;

                    App.DataViewModel.ProgressBarText = "Searching " + searchCriteria.SearchText + " ...";
                }

                searchCriteria.CriteriaType = SearchCriteriaType.What;
                App.DataViewModel.SaveSearchedLocations(txtLocation.Text);
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

        private void DoCurrentSearch(SearchCriteria searchCriteria)
        {
            searchCriteria.LocationType = SearchLocationType.Local;
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
                App.DataViewModel.CurrentLat = searchCriteria.LocationLat;
                App.DataViewModel.CurrentLon = searchCriteria.LocationLong;

                if (string.IsNullOrEmpty(GetSearchTypeText()))
                {
                    searchCriteria.SearchText = App.DataViewModel.SelectedMenuCategory.Code;
                    searchCriteria.NonUSSearchText = App.DataViewModel.SelectedMenuCategory.NonUSCode;
                    App.DataViewModel.ProgressBarText = "Searching " + App.DataViewModel.SelectedMenuCategory.Name + " nearby...";
                }
                else
                {
                    searchCriteria.SearchText = GetSearchTypeText();
                    searchCriteria.NonUSSearchText = searchCriteria.SearchText;

                    App.DataViewModel.ProgressBarText = "Searching " + searchCriteria.SearchText + " nearby...";
                }

                searchCriteria.CriteriaType = SearchCriteriaType.What;
                App.DataViewModel.SaveSearchedLocations(txtLocation.Text);
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


        private string GetSearchTypeText()
        {
            var str = txtSearchType.Text.ToLower().Replace(" ", "");
            if (str == "(e.g.pizza,plumbers)")
            {
                return string.Empty;
            }
            return txtSearchType.Text;
        }

        private void TapSearchType(object sender, GestureEventArgs e)
        {

        }

        private void TapLocation(object sender, GestureEventArgs e)
        {
            var viewModel = App.DataViewModel;
            if (viewModel.StoredSearchLocations.Count > 1)
            {
                panelInfo.Visibility = Visibility.Collapsed;
                listbox.Visibility = Visibility.Visible;
            }
        }
        private void listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listbox.SelectedItem != null)
            {
                if (listbox.SelectedIndex != 0)
                {
                    txtLocation.Text = listbox.SelectedItem as string;
                }
                else
                {
                    txtLocation.Text = string.Empty;
                }

                listbox.Visibility = Visibility.Collapsed;
            }

        }

        private void GridTap(object sender, GestureEventArgs e)
        {
            if ((e.OriginalSource != null) && (e.OriginalSource is Grid))
            {
                listbox.Visibility = Visibility.Collapsed;
                panelInfo.Visibility = Visibility.Visible;
            }
        }

        private void SettingsClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.Xaml", UriKind.RelativeOrAbsolute));
        }

        private void FilterClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Filter.Xaml", UriKind.RelativeOrAbsolute));
        }
    }
}