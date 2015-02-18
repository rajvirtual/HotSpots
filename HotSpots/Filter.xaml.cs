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
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace HotSpots
{
    public partial class Filter : PhoneApplicationPage
    {
        public Filter()
        {
            InitializeComponent();
            DataContext = App.DataViewModel;
            Loaded += new RoutedEventHandler(Filter_Loaded);

        }

        void Filter_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = App.DataViewModel;
            switch (viewModel.SelectedRppType)
            {
                case RppType.Twenty:
                    {
                        rpp20.IsChecked = true;
                        break;
                    }
                case RppType.Thirty:
                    {
                        rpp30.IsChecked = true;
                        break;
                    }
                case RppType.Forty:
                    {
                        rpp40.IsChecked = true;
                        break;
                    }
                case RppType.Fifty:
                    {
                        rpp50.IsChecked = true;
                        break;
                    }
            }

            switch (viewModel.SelectedSortType)
            {
                case SortType.Distance:
                    {
                        sortDistance.IsChecked = true;
                        break;
                    }
                case SortType.Alpha:
                    {
                        sortAlpha.IsChecked = true;
                        break;
                    }
                case SortType.HighestRated:
                    {
                        sortRated.IsChecked = true;
                        break;
                    }
                case SortType.MostReviewed:
                    {
                        sortReviewed.IsChecked = true;
                        break;
                    }
                case SortType.TopMatches:
                    {
                        sortMatches.IsChecked = true;
                        break;
                    }
                case SortType.Offers:
                    {
                        sortOffers.IsChecked = true;
                        break;
                    }
            }

        }

        private void sortChecked(object sender, RoutedEventArgs e)
        {
            var viewModel = App.DataViewModel;
            viewModel.SelectedSortType = (SortType)Enum.Parse(typeof(SortType),
                (string)(sender as RadioButton).Tag, true);
        }

        private void RppClicked(object sender, RoutedEventArgs e)
        {
            var viewModel = App.DataViewModel;
            viewModel.SelectedRppType = (RppType)Enum.Parse(typeof(RppType), (string)(sender as RadioButton).Tag, true);
        }
    }
}