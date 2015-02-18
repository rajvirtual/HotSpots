using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Text;
using System.Windows;
using HotSpots.Bing.Geocode;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Info;
using Microsoft.Phone.Reactive;
using Microsoft.Xna.Framework.GamerServices;
using DeviceType = Microsoft.Devices.DeviceType;

namespace HotSpots
{
    public static class Utils
    {
        private static GeoCoordinateWatcher _watcher;
        private static ShakeDetector _shakeDetector;
        public delegate void SetDetailDataCompletedEvent();
        public static event SetDetailDataCompletedEvent SetDetailDataCompleted;

        public delegate void SetCountryCodeCompletedEvent();
        public static event SetCountryCodeCompletedEvent SetCountryCodeCompleted;
        private static SearchCriteria _searchCriteria;

        public static void SearchListResults(SearchCriteria searchCriteria)
        {
            var viewModel = App.DataViewModel;
            if (viewModel.Busy)
            {
                return;
            }
            _searchCriteria = searchCriteria;



            if (!viewModel.CountryLocated)
            {
                var countryName = GetCountryName();
                if (countryName == string.Empty)
                {
                    viewModel.Busy = true;
                    viewModel.Loaded = false;
                    SetCountryCodeCompleted += Utils_SetCountryCodeCompleted;
                    GetCountryCode(new GeoCoordinate(viewModel.CurrentLat.ToDouble(), viewModel.CurrentLon.ToDouble()));
                }
                else
                {
                    viewModel.USLocation = countryName == "United States";
                    viewModel.CountryName = countryName;
                    viewModel.CountryLocated = true;
                    Utils_SetCountryCodeCompleted();
                }
            }
            else
            {
                Utils_SetCountryCodeCompleted();
            }

        }

        private static void Utils_SetCountryCodeCompleted()
        {
            var viewModel = App.DataViewModel;
            viewModel.Busy = true;
            viewModel.Loaded = false;


            var searchUrl = _searchCriteria.BuildSearchListUrl();
            var request = HttpWebRequest.CreateHttp(searchUrl);

            var observableRequest = Observable.
                FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.
                                              EndGetResponse);
            Observable.Timeout(observableRequest.Invoke(), TimeSpan.FromSeconds(120)).
                Subscribe(response => { HandleListResult(response); },
                exception => { HandleListResultTimeOut(exception); });
        }


        public static string Distance(GeoCoordinate from, GeoCoordinate to)
        {
            return Math.Round(from.GetDistanceTo(to) * .000621371192, 2) + " miles";
        }

        public static void GetCountryCode(GeoCoordinate location)
        {
            var reverseGeocodeRequest = new ReverseGeocodeRequest();
            reverseGeocodeRequest.Credentials = new Credentials();
            reverseGeocodeRequest.Credentials.ApplicationId = App.Id;

            var point = new GeoCoordinate();
            point.Latitude = location.Latitude;
            point.Longitude = location.Longitude;

            reverseGeocodeRequest.Location = point;
            var geocodeService = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
            geocodeService.ReverseGeocodeCompleted += GeocodeService_ReverseGeocodeCompleted;
            geocodeService.ReverseGeocodeAsync(reverseGeocodeRequest);

        }

        public static string GetCountryName()
        {
            if (_userSettings.Contains("Country"))
            {
                return _userSettings["Country"] as string;
            }

            return string.Empty;
        }

        static void GeocodeService_ReverseGeocodeCompleted(object sender, ReverseGeocodeCompletedEventArgs e)
        {
            var viewModel = App.DataViewModel;
            viewModel.Busy = false;
            viewModel.Loaded = true;
            viewModel.USLocation = false;
            viewModel.CountryLocated = true;
            foreach (var result in e.Result.Results)
            {
                if (result.Address.CountryRegion == "United States")
                {
                    viewModel.USLocation = true;
                    viewModel.CountryName = result.Address.CountryRegion;
                }

                viewModel.CountryName = result.Address.CountryRegion;
            }

            if (!_userSettings.Contains("Country"))
            {
                _userSettings.Add("Country", viewModel.CountryName);
            }
            else
            {
                _userSettings["Country"] = viewModel.CountryName;
            }

            _userSettings.Save();


            if (SetCountryCodeCompleted != null)
            {
                SetCountryCodeCompleted();
            }
        }

        public static void SearchDetailResults(string locationId)
        {
            var viewModel = App.DataViewModel;
            viewModel.ProgressBarText = "Loading details...";
            if (viewModel.Busy)
            {
                return;
            }

            viewModel.Busy = true;
            viewModel.Loaded = false;
            string uri =
                     "http://api.citygridmedia.com/content/places/v2/detail?listing_id={0}&publisher=8999798002&client_ip=123.4.56.78&api_key=knr53dcpeug8psty7367br94";
            uri = string.Format(uri, locationId);
            var request = HttpWebRequest.CreateHttp(uri);

            var observableRequest = Observable.
                FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.
                                                                            EndGetResponse);
            Observable.Timeout(observableRequest.Invoke(), TimeSpan.FromSeconds(120)).
                Subscribe(response => { HandleDetailsResult(response); },
                exception => { HandleDetailResultTimeOut(exception); });

        }

        private static void HandleDetailResultTimeOut(Exception exception)
        {
            var viewModel = App.DataViewModel;
            var navigationService = App.CurrentNavigationService;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(
                    "Unable to retrieve data at this time. Please try again later.");
                viewModel.Busy = false;
                viewModel.Loaded = true;
                try
                {
                    navigationService.Navigate(
                    new Uri("/MainPage.xaml",
                    UriKind.RelativeOrAbsolute));
                }
                catch (Exception)
                {

                }

            });
        }

        private static void HandleListResultTimeOut(Exception exception)
        {
            var viewModel = App.DataViewModel;
            var navigationService = App.CurrentNavigationService;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(
                    "Unable to retrieve data at this time. Please try again later.");
                viewModel.Busy = false;
                viewModel.Loaded = true;
                try
                {
                    navigationService.Navigate(
                    new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
                }
                catch (Exception)
                {


                }

            });
        }

        private static void HandleListResult(WebResponse response)
        {
            var viewModel = App.DataViewModel;
            var navigationService = App.CurrentNavigationService;
            using (var streamReader1 = new StreamReader(response.GetResponseStream()))
            {
                string resultString = streamReader1.ReadToEnd();
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    viewModel.SetListData(resultString);

                    viewModel.Busy = false;
                    viewModel.Loaded = true;
                    if (viewModel.TotalHits == 0)
                    {
                        if (viewModel.DidYouMeanText.IsNullOrEmpty())
                        {
                            MessageBox.Show(
                                "No results found. Please try refining your search.");
                        }
                        else
                        {
                            MessageBox.Show(
                                "No results found. Please try refining your search. Did you mean to search for " + "''"
                                + viewModel.DidYouMeanText + "''");
                        }

                        try
                        {
                            navigationService.Navigate(
    new Uri("/MainPage.xaml",
            UriKind.RelativeOrAbsolute));
                        }
                        catch (Exception)
                        {


                        }


                    }
                });
            }

        }

        private static void HandleDetailsResult(WebResponse response)
        {
            var viewModel = App.DataViewModel;
            using (var streamReader1 = new StreamReader(response.GetResponseStream()))
            {
                string resultString = streamReader1.ReadToEnd();
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    viewModel.SetDetail(resultString);
                    SetUpAcceleratorIfRequired();
                    if (SetDetailDataCompleted != null)
                    {
                        SetDetailDataCompleted();
                    }
                    //viewModel.Busy = false;
                    //viewModel.Loaded = true;
                });
            }
        }


        public static void SetUpAcceleratorIfRequired()
        {
            var viewModel = App.DataViewModel;
            if (viewModel.HasOffers == Visibility.Visible)
            {
                _shakeDetector = new ShakeDetector();
                _shakeDetector.ShakeEvent += _shakeDetector_ShakeEvent;
                _shakeDetector.Start();
            }
        }

        public static void StopShakeDetector()
        {
            if (_shakeDetector != null)
            {
                _shakeDetector.Stop();
                _shakeDetector = null;
            }
        }
        private static void _shakeDetector_ShakeEvent(object sender, EventArgs e)
        {
            var navigationService = App.CurrentNavigationService;
            _shakeDetector.Stop();
            _shakeDetector.ShakeEvent -= _shakeDetector_ShakeEvent;
            _shakeDetector = null;
            Deployment.Current.Dispatcher.BeginInvoke(() => navigationService.Navigate(new Uri("/OffersPage.Xaml", UriKind.Relative)));


        }

        private readonly static IsolatedStorageSettings _userSettings = IsolatedStorageSettings.ApplicationSettings;

        public static bool HasAllowedLocationServices
        {
            get
            {
                if (_userSettings.Contains("HotSpotsLocationWarning"))
                {
                    string settings = _userSettings["HotSpotsLocationWarning"].ToString();
                    if (settings == "Yes")
                    {
                        App.DataViewModel.AllowedServiceLocator = true;
                        return true;
                    }
                }

                return false;
            }

        }

        public static void ShowLocationServicesAllowWarning()
        {

            var sb = new StringBuilder();
            sb.Append("Sharing this information helps us provide location based services.We wont use the info to identify or ");
            sb.Append("contact you.Your current location data is required for this feature to work.");
            Guide.BeginShowMessageBox("Allow Hot Spots to access and use your location?", sb.ToString(),
                    new List<string> { "allow", "deny" }, 0, MessageBoxIcon.Error,
                    asyncResult =>
                    {
                        int? returned = Guide.EndShowMessageBox(asyncResult);

                        if (returned.HasValue && returned.Value == 0)
                        {
                            if (!_userSettings.Contains("HotSpotsLocationWarning"))
                            {
                                _userSettings.Add("HotSpotsLocationWarning", "Yes");
                            }
                            else
                            {
                                _userSettings["HotSpotsLocationWarning"] = "Yes";
                            }

                            _userSettings.Save();
                            App.DataViewModel.AllowedServiceLocator = true;
                        }
                    }, null);
        }

        public static void SetLocationServiceData(string data)
        {
            if (_userSettings.Contains("HotSpotsLocationWarning"))
            {
                _userSettings["HotSpotsLocationWarning"] = data;
                _userSettings.Save();
            }
        }

        public static void SetCountryName(string country){
            if (_userSettings.Contains("Country"))
            {
                _userSettings["Country"] = country;
                _userSettings.Save();
            }
        }

        public static string GetLocationServiceData()
        {
            if (_userSettings.Contains("HotSpotsLocationWarning"))
            {
                return _userSettings["HotSpotsLocationWarning"] as string;
            }

            return string.Empty;
        }

        public static GeoCoordinate GetCurrentLocation()
        {
            if (Microsoft.Devices.Environment.DeviceType == DeviceType.Emulator)
            {
                   return new GeoCoordinate(33.855644, -84.476920);
                //return new GeoCoordinate(55.578344672182055, -3.8671875);
            }
            if (!HasAllowedLocationServices)
            {
                ShowLocationServicesAllowWarning();
                return null;
            }

            var viewModel = App.DataViewModel;
            if (viewModel.ServiceLocatorInitialized)
            {
                return new GeoCoordinate(Convert.ToDouble(viewModel.CurrentLat), Convert.ToDouble(viewModel.CurrentLon));
            }
            _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            _watcher.MovementThreshold = 20; // 20 meters. 
            _watcher.StatusChanged += WatcherStatusChanged;

            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    viewModel.Busy = true;
                    viewModel.Loaded = false;
                });

                var started = _watcher.TryStart(true, TimeSpan.FromSeconds(60));
                if (!started)
                {
                    ShowLocationServicesDisabledMessage();
                    _watcher.Stop();
                    return null;
                }
                if (!_watcher.Position.Location.IsUnknown)
                {
                    double latitude = _watcher.Position.Location.Latitude;
                    double longitude = _watcher.Position.Location.Longitude;
                    viewModel.CurrentLat = latitude.ToString("0.0000");
                    viewModel.CurrentLon = longitude.ToString("0.0000");
                    viewModel.ServiceLocatorInitialized = true;
                    _watcher.Stop();
                    return new GeoCoordinate(latitude, longitude);
                }
                _watcher.Stop();
                return null;
            }

            catch
            {
                _watcher.Stop();
                return null;

            }
            finally
            {
                _watcher.Stop();
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    viewModel.Busy = false;
                    viewModel.Loaded = true;
                });
            }
        }
        private static void SetBusyFalse()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                App.DataViewModel.Busy = false;
                App.DataViewModel.Loaded = true;
            });
        }

        private static void WatcherStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    if (_watcher.Permission == GeoPositionPermission.Denied)
                    {
                        //    textBox1.Text += "You have disabled the location Service on your device.\n";
                        _watcher.Stop();
                        ShowLocationServicesDisabledMessage();
                    }
                    else
                    {
                        //textBox1.Text += "Location is not functioning on this device\n";
                        _watcher.Stop();
                        MessageBox.Show("Location service is not functioning on this device.");
                        SetBusyFalse();
                    }
                    break;
                case GeoPositionStatus.Initializing:
                    //   textBox1.Text += "Location service is initializing...\n";
                    break;
                case GeoPositionStatus.NoData:
                    //textBox1.Text += "Location data is not available.\n";
                    _watcher.Stop();
                    MessageBox.Show("Location data is not available at this time.");
                    SetBusyFalse();
                    break;
                case GeoPositionStatus.Ready:
                    //textBox1.Text += "Location data is available.\n";
                    _watcher.Stop();
                    SetBusyFalse();
                    break;
            }

        }

        private static void ShowLocationServicesDisabledMessage()
        {
            var sb = new StringBuilder();
            sb.Append("Location data is not available at this time. Your current location data is required for this feature to work. If your location services are turned off, To ");
            sb.Append("turn them on, go to Settings--> location.");
            Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(sb.ToString()));
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                App.DataViewModel.Busy = false;
                App.DataViewModel.Loaded = true;
            });

        }

        public static string BuildReferenceUrl(string url)
        {
            return url + "?reference_id=" + App.DataViewModel.MoreInfos.ReferenceId + "&publisher=" + App.Publisher;
        }

        public static byte[] GetDeviceUniqueID()
        {
            byte[] result = null;
            object uniqueId;
            if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
                result = (byte[])uniqueId;
            return result;
        }


    }

    public static class Extensions
    {
        public static double ToDouble(this string str)
        {
            var dbl = 0.00;
            var converted = Double.TryParse(str, out dbl);
            if (converted)
            {
                return dbl;
            }
            return 0.00;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }


        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> coll)
        {
            var c = new ObservableCollection<T>();
            foreach (var e in coll)
                c.Add(e);
            return c;
        }


    }

}
