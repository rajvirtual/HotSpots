using System;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Windows;
using System.Windows.Input;
using HotSpots.Bing.Route;
using HotSpots.Helpers;
using HotSpots.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using HotSpots.Helpers;

namespace HotSpots
{

    public partial class MapPage : PhoneApplicationPage
    {
        #region Consts

        /// <value>Default map zoom level.</value>
        private const double DefaultZoomLevel = 8.0;

        /// <value>Maximum map zoom level allowed.</value>
        private const double MaxZoomLevel = 21.0;

        /// <value>Minimum map zoom level allowed.</value>
        private const double MinZoomLevel = 1.0;

        #endregion

        #region Fields

        /// <value>Provides credentials for the map control.</value>
        private readonly CredentialsProvider _credentialsProvider = new ApplicationIdCredentialsProvider(App.Id);

        /// <value>Default location coordinate.</value>
        private static readonly GeoCoordinate DefaultLocation =
            new GeoCoordinate(Convert.ToDouble(App.DataViewModel.CurrentLat), Convert.ToDouble(App.DataViewModel.CurrentLon));

        private static GeoCoordinate SelectedLocation
        {
            get
            {
                if (App.DataViewModel.SelectedLocation != null)
                {
                    return new GeoCoordinate(Convert.ToDouble(App.DataViewModel.SelectedLocation.Lat),
                       Convert.ToDouble(App.DataViewModel.SelectedLocation.Long));
                }
                return DefaultLocation;
            }
        }

        /// <value>Collection of pushpins available on map.</value>
        private readonly ObservableCollection<PushpinModel> _pushpins = new ObservableCollection<PushpinModel>
        {
            new PushpinModel
            {
                Location = SelectedLocation,
                Icon = new Uri("/Resources/Icons/Pushpins/pushpin.png", UriKind.Relative)
            }
        };

        /// <value>Collection of calculated map routes.</value>
        private readonly ObservableCollection<RouteModel> _routes = new ObservableCollection<RouteModel>();

        /// <value>Collection of calculated map routes itineraries.</value>
        private readonly ObservableCollection<ItineraryItem> _itineraries = new ObservableCollection<ItineraryItem>();

        /// <value>Map zoom level.</value>
        private double _zoom;

        /// <value>Map center coordinate.</value>
        private GeoCoordinate _center;

        #endregion

        #region Properties

        public bool HasDirections
        {
            get { return Itineraries.Count > 0; }
        }

        /// <summary>
        /// Gets the credentials provider for the map control.
        /// </summary>
        public CredentialsProvider CredentialsProvider
        {
            get { return _credentialsProvider; }
        }

        /// <summary>
        /// Gets or sets the map zoom level.
        /// </summary>
        public double Zoom
        {
            get { return _zoom; }
            set
            {
                var coercedZoom = Math.Max(MinZoomLevel, Math.Min(MaxZoomLevel, value));
                if (_zoom != coercedZoom)
                {
                    _zoom = value;
                    NotifyPropertyChanged("Zoom");
                }
            }
        }

        /// <summary>
        /// Gets or sets the map center location coordinate.
        /// </summary>
        public GeoCoordinate Center
        {
            get { return _center; }
            set
            {
                if (_center != value)
                {
                    _center = value;
                    NotifyPropertyChanged("Center");
                }
            }
        }

        /// <summary>
        /// Gets a collection of pushpins.
        /// </summary>
        public ObservableCollection<PushpinModel> Pushpins
        {
            get { return _pushpins; }
        }

        /// <summary>
        /// Gets a collection of routes.
        /// </summary>
        public ObservableCollection<RouteModel> Routes
        {
            get { return _routes; }
        }

        /// <summary>
        /// Gets a collection of route itineraries.
        /// </summary>
        public ObservableCollection<ItineraryItem> Itineraries
        {
            get { return _itineraries; }
        }

        /// <summary>
        /// Gets or sets the route destination location.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Gets or sets the route origin location.
        /// </summary>
        public string From { get; set; }

        #endregion

        #region Tasks

        private void InitializeDefaults()
        {
            Zoom = DefaultZoomLevel;
            Center = SelectedLocation;
            From = DefaultLocation.Latitude.ToString() + "," + DefaultLocation.Longitude.ToString();
            To = App.DataViewModel.SelectedLocation.Address;
        }

        private void ChangeMapMode()
        {
            if (Map.Mode is AerialMode)
            {
                Map.Mode = new RoadMode();
            }
            else
            {
                Map.Mode = new AerialMode(true);
            }
        }

        private void CenterLocation()
        {
            // Center map to default location.
            Center = SelectedLocation;

            // Reset zoom default level.
            Zoom = DefaultZoomLevel;
        }


        private void CreateNewPushpin(object selectedItem, Point point)
        {
            // Translate the map viewport touch point to a geo coordinate.
            GeoCoordinate location;
            Map.TryViewportPointToLocation(point, out location);

            // Use the geo coordinate calculated to add a new pushpin,
            // based on the selected pushpin prototype,
            var pushpinPrototype = selectedItem as PushpinModel;
            var pushpin = pushpinPrototype.Clone(location);
            Pushpins.Add(pushpin);
        }

        private void Pushpin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var pushpin = sender as Pushpin;

            // Center the map on a pushpin when touched.
            Center = pushpin.Location;
        }

        private void CalculateRoute()
        {
            try
            {
                var routeCalculator = new RouteCalculator(
                    CredentialsProvider,
                    To,
                    From,
                    Dispatcher,
                    result =>
                    {
                        // Clear the route collection to have only one route at a time.
                        Routes.Clear();

                        // Clear previous route related itineraries.
                        Itineraries.Clear();

                        // Create a new route based on route calculator result,
                        // and add the new route to the route collection.
                        var routeModel = new RouteModel(result.Result.RoutePath.Points);
                        Routes.Add(routeModel);

                        // Add new route itineraries to the itineraries collection.
                        foreach (var itineraryItem in result.Result.Legs[0].Itinerary)
                        {
                            Itineraries.Add(itineraryItem);
                        }

                        // Set the map to center on the new route.
                        var viewRect = LocationRect.CreateLocationRect(routeModel.Locations);
                        Map.SetView(viewRect);

                        ShowDirectionsView();
                    });

                // Display an error message in case of fault.
                routeCalculator.Error += r => MessageBox.Show(r.Reason);

                // Start the route calculation asynchronously.
                routeCalculator.CalculateAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

    }
}