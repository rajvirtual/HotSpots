using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Advertising.Mobile.UI;
using Microsoft.Phone.Controls;


namespace HotSpots
{
    public partial class ListResults : PhoneApplicationPage
    {
        private ViewModel _viewModel;
        public ListResults()
        {
            InitializeComponent();
            DataContext = App.DataViewModel;
            _viewModel = App.DataViewModel;
            listbox.SelectedIndex = -1;
            Loaded += new RoutedEventHandler(ListResults_Loaded);
        }

        void ListResults_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = App.DataViewModel;
            if (!viewModel.CurrentLat.IsNullOrEmpty())
            {
                //if (adListResults.Location.Latitude <= 0)
                //{
                //    //adListResults.L
                //    //adListResults.Location = new Location(viewModel.CurrentLat.ToDouble(),
                //                                          //viewModel.CurrentLon.ToDouble());
                //}
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            listbox.SelectedIndex = -1;
            base.OnNavigatedTo(e);
        }

        private void ListResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel == null)
            {
                return;
            }
            if (_viewModel.Busy)
            {
                return;

            }

            var selectedItem = _viewModel.SelectedLocation;
            if (!_viewModel.USLocation)
            {
                if (selectedItem != null)
                {
                    NavigationService.Navigate(new Uri("/Details.Xaml", UriKind.Relative));
                    return;
                }
            }

            if (selectedItem != null)
            {
                Utils.SetDetailDataCompleted += Utils_SetDetailDataCompleted;
                Utils.SearchDetailResults(selectedItem.LocationId);
            }

        }

        private void Utils_SetDetailDataCompleted()
        {
            var selectedItem = _viewModel.SelectedLocation;
            if (selectedItem != null)
            {
                var url = "/Details.Xaml?locationid=" + selectedItem.LocationId;
                try
                {
                    NavigationService.Navigate(new Uri(url, UriKind.Relative));

                }
                catch (Exception)
                {
                    _viewModel.Busy = false;
                    _viewModel.Loaded = true;
                }
                finally
                {

                }

            }

        }

        private void TapSearch(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Search.xaml", UriKind.RelativeOrAbsolute));
        }

    }


}