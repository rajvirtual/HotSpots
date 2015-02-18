using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Xml.Linq;
using HotSpots.Objects;

namespace HotSpots
{
    public class ListData
    {
        public string Counter { get; set; }
        public string Name { get; set; }
        public string ReviewCount { get; set; }
        public string ReviewNumber { get; set; }
        public string LocationId { get; set; }
        public string Address { get; set; }
        public string Miles { get; set; }
        public string Category { get; set; }
        public string Neighbourhood { get; set; }
        public string ImgSource { get; set; }
        public CardinalPoints Direction { get; set; }
        public string DirectionImgSource { get; set; }
        public double RatingValue { get; set; }
        public string RatingText { get; set; }
        public string RatingImageUrl { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Category> Categories { get; set; }
        public Visibility USVisibility { get; set; }
        public Visibility NonUSVisibility { get; set; }
        public string MobileWebSiteUrl { get; set; }
        private string _phone;
        public string Phone
        {
            get
            {
                {
                    if (_phone == string.Empty)
                    {
                        return string.Empty;
                    }
                    if (_phone.Contains("-"))
                    {
                        return _phone;
                    }
                    double number;
                    if (Double.TryParse(_phone, out number))
                    {
                        return string.Format("{0:(###) ###-####}", Convert.ToInt64(_phone));
                    }

                    return _phone;

                }
            }
            set { _phone = value; }
        }
        public string Lat { get; set; }
        public string Long { get; set; }

    }

    public class ReviewData
    {
        public string ReviewOwner { get; set; }
        public string ReviewDate { get; set; }
        public string ReviewText { get; set; }
        public double RatingValue { get; set; }
        public string CitySearchUrl { get; set; }
        public string ReviewOwnerPhotoUrl { get; set; }
    }

    public class EditorialData
    {
        public string Title { get; set; }
        public string CitySearchUrl { get; set; }
        public string Author { get; set; }
        public string ReviewText { get; set; }
        public string ReviewDate { get; set; }
        public double RatingValue { get; set; }
    }

    public class ListFavourite : INotifyPropertyChanged
    {
        private string _counter;
        public string Counter
        {
            get { return _counter; }
            set
            {
                _counter = value;
                NotifyPropertyChanged("Counter");
            }
        }
        public bool IsSelected { get; set; }
        public string Code { get; set; }
        public string ImgSource
        {
            get { return "/Images/Favourites.png"; }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    public class AttributesData
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class TipsData
    {
        public string Name { get; set; }
        public string Text { get; set; }
    }

    public class MenuCategory
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string NonUSCode { get; set; }
        public string Image { get; set; }
        public SearchCriteriaType CriteriaType { get; set; }
        public NonUSSearchCriteriaType NonUSCriteriaType { get; set; }
    }

    public class OfferData
    {
        public string Text { get; set; }
        public string Description { get; set; }
    }

    public class LinksData
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
    }


    public enum SortType { Distance, Alpha, HighestRated, MostReviewed, TopMatches, Offers }
    public enum RppType { Twenty, Thirty, Forty, Fifty }


    public class MoreInfoData
    {
        public string AboutData { get; set; }
        public string AttributionText { get; set; }
        public string CustomerMessageUrl { get; set; }
        public string Categories { get; set; }
        public string Cuisines { get; set; }
        public string ReferenceId { get; set; }
        public string HairCareServices { get; set; }
        public string PaymentMethods { get; set; }
        public string SpecialFeatures { get; set; }
        public List<AttributesData> SpecialAttributes { get; set; }
        public List<TipsData> Tips { get; set; }
        public string BusinessHours { get; set; }
        public string Neighbourhoods { get; set; }
        public string Metro { get; set; }
        public string MenuUrl { get; set; }
        public string WebSiteUrl { get; set; }
        public List<string> ImageUrls { get; set; }
        public int ImageCount { get; set; }
        public string VideoUrl { get; set; }
        public string ReservationUrl { get; set; }
        public string GenInfoLabel { get; set; }
        public string GenInfoData { get; set; }
        public List<OfferData> Offers { get; set; }
        public List<LinksData> Links { get; set; }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        public bool USLocation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public SortType SelectedSortType { get; set; }

        public string PostMessageCaption
        {
            get;
            set;
        }

        public string PostMessageText
        {
            get;
            set;
        }

        public int PostMessageMaxLength
        {
            get;
            set;
        }




        public RppType SelectedRppType { get; set; }

        public bool SearchForOffers { get; set; }

        public Dictionary<SortType, string> SortTypes = new Dictionary<SortType, string>() 
        { { SortType.Distance, "dist" },
         { SortType.Alpha, "alpha" },
         { SortType.HighestRated, "highestrated" },
         { SortType.MostReviewed, "mostreviewed" },
         { SortType.TopMatches, "topmatches" },
         { SortType.Offers, "offers" }};

        public string GetSelectedSortType()
        {
            return SortTypes[SelectedSortType];
        }

        public string GetSelectedRppType()
        {
            switch (SelectedRppType)
            {
                case RppType.Twenty:
                    return "20";
                case RppType.Thirty:
                    return "30";
                case RppType.Forty:
                    return "40";
                case RppType.Fifty:
                    return "50";
            }

            return "20";
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public string SearchListUrl
        {
            get;
            set;
        }

        private string _progressBarText;
        public string ProgressBarText
        {
            get { return _progressBarText; }
            set
            {
                _progressBarText = value;
                NotifyPropertyChanged("ProgressBarText");
            }
        }
        public List<MenuCategory> MenuCategories
        {
            get;
            set;
        }


        private MenuCategory _selectedMenuCategory;
        public MenuCategory SelectedMenuCategory
        {
            get { return _selectedMenuCategory; }
            set
            {
                _selectedMenuCategory = value;
                NotifyPropertyChanged("SelectedMenuCategory");
            }
        }


        public string CurrentLon
        {
            get;
            set;
        }

        public string CurrentLat
        {
            get;
            set;
        }

        public string SearchLon
        {
            get;
            set;
        }

        public string SearchLat
        {
            get;
            set;
        }

        public bool AllowedServiceLocator { get; set; }
        public bool ServiceLocatorInitialized { get; set; }
        public int TotalHits { get; set; }

        public ViewModel()
        {
            LoadCategories();
            CountryName = Utils.GetCountryName();
            StoredSearchLocations = new ObservableCollection<string>();
            Favourites = new ObservableCollection<ListFavourite>();
            SearchForOffers = false;
            SelectedRppType = RppType.Twenty;
            SelectedSortType = SortType.Distance;
            LoadStoredSearchLocations();


        }

        public bool CountryLocated { get; set; }

        public void LoadCategories()
        {

            MenuCategories = new List<MenuCategory>();
            var menuCategory = new MenuCategory();
            menuCategory.Name = "Restaurants";
            menuCategory.Code = "restaurant";
            menuCategory.NonUSCode = "restaurants";
            menuCategory.NonUSCriteriaType = NonUSSearchCriteriaType.Category;
            menuCategory.Image = "Images/Restaurant.png";
            menuCategory.CriteriaType = SearchCriteriaType.What;
            MenuCategories.Add(menuCategory);

            menuCategory = new MenuCategory();
            menuCategory.Name = "Cafes";
            menuCategory.Code = "cafes";
            menuCategory.NonUSCode = "cafes";
            menuCategory.Image = "Images/Cafe.png";
            menuCategory.NonUSCriteriaType = NonUSSearchCriteriaType.Term;
            menuCategory.CriteriaType = SearchCriteriaType.What;
            MenuCategories.Add(menuCategory);


            menuCategory = new MenuCategory();
            menuCategory.Name = "Bar/Clubs";
            menuCategory.Code = "barclub";
            menuCategory.NonUSCode = "bars";
            menuCategory.CriteriaType = SearchCriteriaType.Type;
            menuCategory.NonUSCriteriaType = NonUSSearchCriteriaType.Category;
            menuCategory.Image = "Images/Bar.png";
            MenuCategories.Add(menuCategory);

            menuCategory = new MenuCategory();
            menuCategory.Name = "Salon & Spas";
            menuCategory.Code = "salons";
            menuCategory.NonUSCode = "beautysvc";
            menuCategory.CriteriaType = SearchCriteriaType.What;
            menuCategory.NonUSCriteriaType = NonUSSearchCriteriaType.Category;
            menuCategory.Image = "Images/Salon.png";
            MenuCategories.Add(menuCategory);

            menuCategory = new MenuCategory();
            menuCategory.Name = "Movies";
            menuCategory.NonUSCode = "arts";
            menuCategory.Code = "movietheater";
            menuCategory.CriteriaType = SearchCriteriaType.Type;
            menuCategory.NonUSCriteriaType = NonUSSearchCriteriaType.Category;
            menuCategory.Image = "Images/movie.png";
            MenuCategories.Add(menuCategory);



            menuCategory = new MenuCategory();
            menuCategory.Name = "Travels";
            menuCategory.Code = "travel";
            menuCategory.NonUSCode = "transport";
            menuCategory.Image = "Images/Travel.png";
            menuCategory.CriteriaType = SearchCriteriaType.What;
            menuCategory.NonUSCriteriaType = NonUSSearchCriteriaType.Category;
            MenuCategories.Add(menuCategory);

            menuCategory = new MenuCategory();
            menuCategory.Name = "Favourites";
            menuCategory.Code = "favourite";
            menuCategory.NonUSCode = "favourite";
            menuCategory.Image = "Images/Favourites.png";
            menuCategory.CriteriaType = SearchCriteriaType.What;
            MenuCategories.Add(menuCategory);

            menuCategory = new MenuCategory();
            menuCategory.Name = "Hotels";
            menuCategory.Code = "hotels";
            menuCategory.NonUSCode = "hotels";
            menuCategory.CriteriaType = SearchCriteriaType.What;
            menuCategory.NonUSCriteriaType = NonUSSearchCriteriaType.Term;
            menuCategory.Image = "Images/Hotel.png";
            MenuCategories.Add(menuCategory);


            menuCategory = new MenuCategory();
            menuCategory.Name = "Banks";
            menuCategory.Code = "banks";
            menuCategory.NonUSCode = "banks";
            menuCategory.CriteriaType = SearchCriteriaType.What;
            menuCategory.NonUSCriteriaType = NonUSSearchCriteriaType.Term;
            menuCategory.Image = "Images/bank.png";
            MenuCategories.Add(menuCategory);


            menuCategory = new MenuCategory();
            menuCategory.Name = "Pharmacies";
            menuCategory.Code = "pharmacy";
            menuCategory.NonUSCode = "pharmacy";
            menuCategory.CriteriaType = SearchCriteriaType.What;
            menuCategory.NonUSCriteriaType = NonUSSearchCriteriaType.Term;
            menuCategory.Image = "Images/Pharmacy.png";
            MenuCategories.Add(menuCategory);

            menuCategory = new MenuCategory();
            menuCategory.Name = "Shopping";
            menuCategory.Code = "shopping";
            menuCategory.NonUSCode = "shopping";
            menuCategory.Image = "Images/Shopping.png";
            menuCategory.CriteriaType = SearchCriteriaType.What;
            menuCategory.NonUSCriteriaType = NonUSSearchCriteriaType.Category;
            MenuCategories.Add(menuCategory);

            NotifyPropertyChanged("MenuCategories");
            SelectedMenuCategory = MenuCategories[0];
            NotifyPropertyChanged("SelectedMenuCategory");
        }

        private ListData _selectedLocation;
        public ListData SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                _selectedLocation = value;
                NotifyPropertyChanged("SelectedLocation");
            }
        }

        private ReviewData _selectedReview;
        public ReviewData SelectedReview
        {
            get { return _selectedReview; }
            set
            {
                _selectedReview = value;
                NotifyPropertyChanged("SelectedReview");
            }
        }

        private EditorialData _selectedEditorial;
        public EditorialData SelectedEditorial
        {
            get { return _selectedEditorial; }
            set
            {
                _selectedEditorial = value;
                NotifyPropertyChanged("SelectedEditorial");
            }
        }


        bool _loaded;
        public bool Loaded
        {
            get
            {
                return _loaded;
            }
            set
            {
                _loaded = value;
                NotifyPropertyChanged("Loaded");
            }
        }

        bool _busy;
        public bool Busy
        {
            get
            {
                return _busy;
            }
            set
            {
                _busy = value;
                NotifyPropertyChanged("Busy");
            }
        }

        public ObservableCollection<ListData> ListResults { get; set; }
        public ObservableCollection<ReviewData> Reviews { get; set; }
        public ObservableCollection<EditorialData> Editorials { get; set; }

        private readonly CollectionViewSource _metrosCollection = new CollectionViewSource();

        public ICollectionView MetrosSource
        {
            get { return _metrosCollection.View; }
            set
            {
                NotifyPropertyChanged("MetrosSource");
            }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
            }
        }

        public MoreInfoData MoreInfos { get; set; }
        public string DidYouMeanText { get; set; }
        public Visibility USVisibility { get; set; }
        public Visibility NonUSVisibility { get; set; }

        private void SetNoUSListData(string data)
        {

            var ms = new MemoryStream(Encoding.Unicode.GetBytes(data));
            var serializer = new DataContractJsonSerializer(typeof(SearchResult));
            var result = (SearchResult)serializer.ReadObject(ms);
            int counter = 0;
            SearchLat = CurrentLat;
            SearchLon = CurrentLon;
            ListResults = new ObservableCollection<ListData>();
            TotalHits = result.Businesses.Count();
            foreach (var l in result.Businesses)
            {
                var degrees =
                    DistanceHelper.Bearing(new Coordinate { Latitude = SearchLat.ToDouble(), Longitude = SearchLon.ToDouble() },
                                           new Coordinate { Latitude = l.Latitude.Value, Longitude = l.Longitude.Value });

                var direction = degrees.ToCardinalMark();

                var listData = new ListData()
                {
                    Counter = (++counter).ToString() + ".",
                    Name = l.Name,
                    Phone = l.PhoneNumber,
                    ReviewCount = l.ReviewCount.ToString() + " Reviews",
                    ReviewNumber = string.IsNullOrEmpty(l.ReviewCount.ToString()) ? "0" : l.ReviewCount.ToString(),
                    LocationId = l.Id,
                    Lat = l.Latitude.Value.ToString("0.0000"),
                    Long = l.Longitude.Value.ToString("0.0000"),
                    Address =
                        l.Address1 + ", " + l.City + ", " +
                        l.State + " " + l.ZipCode,
                    Miles = Utils.Distance(new GeoCoordinate(SearchLat.ToDouble(), SearchLon.ToDouble()),
                    new GeoCoordinate(l.Latitude.Value, l.Longitude.Value)),
                    Direction = direction,
                    ImgSource = l.PhotoUrl.AbsoluteUri.Contains("blank_biz_") ? SelectedMenuCategory.Image : l.PhotoUrl.AbsoluteUri,
                    DirectionImgSource = GetDirectionImgSource(DistanceHelper.ToCardinalMark(degrees)),
                    RatingValue = string.IsNullOrEmpty(l.AverageRating.ToString()) ? 0 : l.AverageRating / 5,
                    RatingImageUrl = l.RatingImageUrlSmall.AbsoluteUri,
                    RatingText = l.AverageRating.ToString() + " Ratings",
                    Reviews = l.Reviews,
                    Categories = l.Categories,
                    USVisibility =  USVisibility,
                    NonUSVisibility = NonUSVisibility,
                    MobileWebSiteUrl = l.MobileUrl.AbsoluteUri
                };

                ListResults.Add(listData);

                NotifyPropertyChanged("USVisibility");
                NotifyPropertyChanged("NonUSVisibility");
            }


            ListResults = ApplySort(ListResults).ToObservableCollection();
            ListResults = ReIndex(ListResults).ToObservableCollection();


            NotifyPropertyChanged("ListResults");


        }

        private string _countryName;
        public string CountryName
        {
            get { return _countryName.IsNullOrEmpty()? "Not Assigned":_countryName; }
            set
            {
                _countryName = value;
                NotifyPropertyChanged("CountryName");
            }
        }

        private List<ListData> ReIndex(ObservableCollection<ListData> list){
            for (int i = 0; i < list.Count; i++)
            {

                list[i].Counter = (i+1).ToString() + ".";
            }

            return list.ToList();
        }

        private List<ListData> ApplySort(ObservableCollection<ListData> list)
        {
            switch (SelectedSortType)
            {
                case SortType.Distance:
                    {
                        return list.OrderBy(l => l.Miles.Substring(0, l.Miles.IndexOf("miles") - 1).ToDouble()).ToList();

                    }
                case SortType.Alpha:
                    {
                        return list.OrderBy(l => l.Name).ToList(); ;

                    }
                case SortType.HighestRated:
                    {
                        return list.OrderByDescending(l => l.RatingValue).ToList();
                    }
                case SortType.MostReviewed:
                    {
                        return list.OrderByDescending(l => Convert.ToInt32(l.Reviews.Count())).ToList();
                    }
            }

            return list.OrderBy(l => Convert.ToInt32(l.Miles)).ToList();
        }

        public void SetListData(string data)
        {
            USVisibility = Visibility.Visible;
            NonUSVisibility = Visibility.Collapsed;
            if (!USLocation)
            {
                USVisibility = Visibility.Collapsed;
                NonUSVisibility = Visibility.Visible;
                SetNoUSListData(data);
                return;
            }


            NotifyPropertyChanged("USVisibility");
            NotifyPropertyChanged("NonUSVisibility");
            ListResults = new ObservableCollection<ListData>();
            var doc = XDocument.Parse(data);
            var totalHits = from location in doc.Descendants("results")
                            select (string)location.Attribute("total_hits");


            TotalHits = Convert.ToInt32(totalHits.FirstOrDefault());
            DidYouMeanText = "";

            var didyoumean = from location in doc.Descendants("results")
                             select (string)location.Element("did_you_mean");

            DidYouMeanText = didyoumean.FirstOrDefault();

            var searchLat = from location in doc.Descendants("region")
                            select (string)location.Element("latitude");
            if (searchLat.FirstOrDefault() != null)
            {
                SearchLat = searchLat.FirstOrDefault();
            }
            else
            {
                SearchLat = CurrentLat;
            }

            var searchLong = from location in doc.Descendants("region")
                             select (string)location.Element("longitude");
            if (searchLong.FirstOrDefault() != null)
            {
                SearchLon = searchLong.FirstOrDefault();
            }
            else
            {
                SearchLon = CurrentLon;
            }


            var locations = from location in doc.Descendants("location")
                            select new
                            {

                                Name = (string)location.Element("name"),
                                Street = (from s in location.Elements("address")
                                          select (string)s.Element("street")),
                                City = (from s in location.Elements("address")
                                        select (string)s.Element("city")),
                                State = (from s in location.Elements("address")
                                         select (string)s.Element("state")),
                                Zip = (from s in location.Elements("address")
                                       select (string)s.Element("postal_code")),
                                ReviewCount = (string)location.Element("user_review_count"),
                                LocationId = (string)location.Attribute("id"),
                                Category = (string)location.Element("sample_categories"),
                                Lat = (string)location.Element("latitude"),
                                Long = (string)location.Element("longitude"),
                                Neighbourhood = (string)location.Element("neighborhood"),
                                Phone = (string)location.Element("phone_number"),
                                Rating = (location.Element("rating") != null) ? (string)location.Element("rating") : "0",
                                ImageSource = (!string.IsNullOrEmpty((string)location.Element("image"))) ? (string)location.Element("image") : SelectedMenuCategory.Image
                            };
            int counter = 0;
            foreach (var l in locations)
            {
                var degrees =
                    DistanceHelper.Bearing(new Coordinate { Latitude = SearchLat.ToDouble(), Longitude = SearchLon.ToDouble() },
                                           new Coordinate { Latitude = l.Lat.ToDouble(), Longitude = l.Long.ToDouble() });

                var direction = degrees.ToCardinalMark();

                var listData = new ListData()
                {
                    Counter = (++counter).ToString() + ".",
                    Name = l.Name,
                    Phone = l.Phone,
                    ReviewCount = l.ReviewCount + " Reviews",
                    ReviewNumber = string.IsNullOrEmpty(l.ReviewCount) ? "0" : l.ReviewCount,
                    LocationId = l.LocationId,
                    Lat = l.Lat,
                    Long = l.Long,
                    Address =
                        l.Street.FirstOrDefault() + ", " + l.City.FirstOrDefault() + ", " +
                        l.State.FirstOrDefault() + " " + l.Zip.FirstOrDefault(),
                    Category = l.Category,
                    Miles = Utils.Distance(new GeoCoordinate(SearchLat.ToDouble(), SearchLon.ToDouble()),
                    new GeoCoordinate(l.Lat.ToDouble(), l.Long.ToDouble())),
                    Neighbourhood = l.Neighbourhood,
                    Direction = direction,
                    ImgSource = l.ImageSource,
                    DirectionImgSource = GetDirectionImgSource(DistanceHelper.ToCardinalMark(degrees)),
                    RatingValue = string.IsNullOrEmpty(l.Rating) ? 0 : l.Rating.ToDouble() / 10,
                    USVisibility = USVisibility,
                    NonUSVisibility = NonUSVisibility,
                    RatingText = l.Rating + " Ratings"
                };

                ListResults.Add(listData);
            }
            NotifyPropertyChanged("ListResults");
        }

        private string GetDirectionImgSource(CardinalPoints direction)
        {
            switch (direction)
            {
                case CardinalPoints.N:
                    {
                        return "Images/North.png";
                        break;
                    }
                case CardinalPoints.S:
                    {
                        return "Images/South.png";
                        break;
                    }
                case CardinalPoints.E:
                    {
                        return "Images/East.png";
                        break;
                    }
                case CardinalPoints.W:
                    {
                        return "Images/West.png";
                        break;
                    }
                case CardinalPoints.SW:
                    {
                        return "Images/SouthWest.png";
                        break;
                    }
                case CardinalPoints.NW:
                    {
                        return "Images/NorthWest.png";
                        break;
                    }
                case CardinalPoints.SE:
                    {
                        return "Images/SouthEast.png";
                        break;
                    }
                case CardinalPoints.NE:
                    {
                        return "Images/NorthEast.png";
                        break;
                    }

            }

            return "Images/South.png";
        }

        private static string GetJoinedString(IEnumerable collection)
        {
            string tempResult = string.Empty;
            int i = 0;
            foreach (var colData in collection)
            {

                if ((i != 0) && (i % 3 == 0))
                {

                    tempResult = tempResult + "\r\n" + colData;

                }
                else
                {
                    if (tempResult.Length > 0)
                    {
                        tempResult = tempResult + " , " + colData;
                    }
                    else
                    {
                        tempResult = (string)colData;
                    }
                }
                i++;
            }

            if (tempResult == string.Empty)
            {
                tempResult = "N/A";
            }

            return tempResult;
        }


        public Visibility HasViewMenuData { get; set; }
        public Visibility HasReservationData { get; set; }
        public Visibility HasPhoneNumber { get; set; }
        private Visibility _hasOffers = Visibility.Collapsed;
        public Visibility HasOffers
        {
            get { return _hasOffers; }
            set
            {
                _hasOffers = value;
                NotifyPropertyChanged("HasOffers");
            }
        }

        public void SetMoreInfoData(string data)
        {
            MoreInfos = new MoreInfoData();

            var doc = XDocument.Parse(data);

            var referenceId = from location in doc.Descendants("location")
                              select (string)location.Element("reference_id");
            if (referenceId.FirstOrDefault() != null)
            {
                MoreInfos.ReferenceId = referenceId.FirstOrDefault();
            }


            var aboutData = from customerContent in doc.Descendants("customer_content")
                            from customerMessage in customerContent.Elements("customer_message")
                            select new
                            {
                                Name = customerMessage.Value
                            };


            var customerMessageUrl = from customerContent in doc.Descendants("customer_content")
                                     from customerMessage in customerContent.Elements("customer_message_url")
                            select new
                            {
                                Name = customerMessage.Value
                            };

            if (customerMessageUrl.FirstOrDefault() != null){
                MoreInfos.CustomerMessageUrl = customerMessageUrl.FirstOrDefault().Name; 
            }



            var straboutData = (aboutData.FirstOrDefault() != null) ? StripHtmlTags(aboutData.FirstOrDefault().Name) : string.Empty;
            straboutData = straboutData.Length > 500 ? straboutData.Substring(0, 495) + "...." : straboutData;
            MoreInfos.AboutData = straboutData;


            var attributionText = from customerContent in doc.Descendants("customer_content")
                                  from customerMessage in customerContent.Elements("customer_message")
                                  select (string)customerMessage.Attribute("attribution_text");

            if (!attributionText.FirstOrDefault().IsNullOrEmpty())
            {
                MoreInfos.AttributionText = "provided by " + attributionText.FirstOrDefault();
            }
            else
            {
                MoreInfos.AttributionText = string.Empty;
            }
            var allCategories = from categoryGroup in doc.Descendants("category")
                                select categoryGroup;

            var categories = from categoryGroup in doc.Descendants("category")
                             select (string)categoryGroup.Attribute("parent");

            MoreInfos.Categories = String.Join(",", categories.Distinct().ToArray());

            var cuisines = from category in doc.Descendants("category")
                           from grp in category.Descendants("group")
                           where (string)grp.Attribute("name") == "Cuisine"
                           select (string)category.Attribute("name");



            MoreInfos.Cuisines = GetJoinedString(cuisines);

            var hairCareServices = from category in doc.Descendants("category")
                                   from grp in category.Descendants("group")
                                   where (string)grp.Attribute("name") == "Hair Care Services"
                                   select (string)category.Attribute("name");


            MoreInfos.HairCareServices = GetJoinedString(hairCareServices);

            if (SelectedMenuCategory.Code == "salon")
            {
                MoreInfos.GenInfoLabel = "Hair Care Services";
                MoreInfos.GenInfoData = MoreInfos.HairCareServices;
            }
            else if (SelectedMenuCategory.Code == "restaurant")
            {
                MoreInfos.GenInfoLabel = "Cuisines";
                MoreInfos.GenInfoData = MoreInfos.Cuisines;
            }
            else
            {
                MoreInfos.GenInfoLabel = null;
                MoreInfos.GenInfoData = null;
            }

            var paymentMethods = from category in doc.Descendants("category")
                                 from grp in category.Descendants("group")
                                 where (string)grp.Attribute("name") == "Payment Methods"
                                 select (string)category.Attribute("name");


            MoreInfos.PaymentMethods = GetJoinedString(paymentMethods);

            var features = from category in allCategories
                           from grp in category.Descendants("group")
                           where (string)grp.Attribute("name") == "Restaurant Special Features"
                           select (string)category.Attribute("name");


            MoreInfos.SpecialFeatures = GetJoinedString(features);

            MoreInfos.SpecialAttributes = new List<AttributesData>();
            var attributes = from locationgroup in doc.Descendants("attribute")
                             select locationgroup;
            foreach (var attribute in attributes)
            {
                var attribData = new AttributesData();
                attribData.Name = (string)attribute.Attribute("name");
                attribData.Value = (string)attribute.Attribute("value");
                MoreInfos.SpecialAttributes.Add(attribData);
            }

            MoreInfos.Tips = new List<TipsData>();
            var tips = from locationgroup in doc.Descendants("tips")
                       from tip in locationgroup.Elements("tip")
                       select tip;

            foreach (var tip in tips)
            {
                var tipData = new TipsData();
                tipData.Name = (string)tip.Element("tip_name");
                tipData.Text = (string)tip.Element("tip_name") + " ? " + "\r\n" + (string)tip.Element("tip_text");
                MoreInfos.Tips.Add(tipData);
            }
            if (MoreInfos.Tips.Count == 0)
            {
                MoreInfos.Tips.Add(new TipsData { Name = "N/A", Text = "N/A" });
            }

            MoreInfos.Offers = new List<OfferData>();

            var offers = from locationgroup in doc.Descendants("offers")
                         from offer in locationgroup.Elements("offer")
                         select offer;
            foreach (var offer in offers)
            {
                var offerData = new OfferData();
                if (offer.HasElements)
                {
                    offerData.Text = (string)offer.Element("offer_text");
                    offerData.Description = (string)offer.Element("offer_description");
                }
                else
                {
                    offerData.Description = offer.Value;
                }

                MoreInfos.Offers.Add(offerData);
            }

            HasOffers = MoreInfos.Offers.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            NotifyPropertyChanged("HasOffers");


            var businessHours = from businessHrs in doc.Descendants("business_hours")
                                select businessHrs;
            MoreInfos.BusinessHours = (businessHours.FirstOrDefault() != null)
                                          ? businessHours.FirstOrDefault().Value
                                          : string.Empty;
            if (MoreInfos.BusinessHours == string.Empty)
            {
                MoreInfos.BusinessHours = "N/A";
            }

            MoreInfos.ImageUrls = new List<string>();
            var imageUrls = from tempImageUrls in doc.Descendants("images")
                            from image in tempImageUrls.Elements("image")
                            select image;
            foreach (var imageUrl in imageUrls)
            {
                MoreInfos.ImageUrls.Add((string)imageUrl.Element("image_url"));
            }

            MoreInfos.ImageCount = MoreInfos.ImageUrls.Count;

            HasViewMenuData = MoreInfos.MenuUrl == string.Empty ? Visibility.Collapsed : Visibility.Visible;

            var videoUrls = from menuUrl in doc.Descendants("urls")
                            select menuUrl.Element("video_url");
            MoreInfos.VideoUrl = (videoUrls.FirstOrDefault() != null) ?
                                videoUrls.FirstOrDefault().Value : string.Empty;


            var links = new List<LinksData>();

            var webSiteUrls = from webSiteUrl in doc.Descendants("urls")
                              select (string)webSiteUrl.Element("website_url");

            if (!string.IsNullOrEmpty(webSiteUrls.FirstOrDefault()))
            {
                var linkData = new LinksData();
                linkData.Text = "Website";
                linkData.Url = webSiteUrls.FirstOrDefault();
                linkData.Image = "Images/Website.png";
                links.Add(linkData);
            }


            var profileUrls = from profileUrl in doc.Descendants("urls")
                              select (string)profileUrl.Element("profile_url");

            if (!string.IsNullOrEmpty(profileUrls.FirstOrDefault()))
            {
                var linkData = new LinksData();
                linkData.Text = "Profile";
                linkData.Url = profileUrls.FirstOrDefault();
                linkData.Image = "Images/Profile.png";
                links.Add(linkData);
            }


            var reviewUrls = from reviewUrl in doc.Descendants("urls")
                             select (string)reviewUrl.Element("reviews_url");

            if (!string.IsNullOrEmpty(reviewUrls.FirstOrDefault()))
            {
                var linkData = new LinksData();
                linkData.Text = "Reviews";
                linkData.Url = reviewUrls.FirstOrDefault();
                linkData.Image = "Images/Reviews.png";
                links.Add(linkData);
            }

            var menuUrls = from menuUrl in doc.Descendants("urls")
                           select (string)menuUrl.Element("menu_url");

            if (!string.IsNullOrEmpty(menuUrls.FirstOrDefault()))
            {
                var linkData = new LinksData();
                linkData.Text = "Menu";
                linkData.Url = menuUrls.FirstOrDefault();
                linkData.Image = "Images/Menu.png";
                links.Add(linkData);
            }


            var reservationUrls = from reservationUrl in doc.Descendants("urls")
                                  select (string)reservationUrl.Element("reservation_url");

            if (!reservationUrls.FirstOrDefault().IsNullOrEmpty())
            {
                var linkData = new LinksData();
                linkData.Text = "Reservation";
                linkData.Url = reservationUrls.FirstOrDefault();
                linkData.Image = "Images/Reservation.png";
                links.Add(linkData);
            }

            var fbLink = new LinksData();
            fbLink.Text = "Post to wall";
            fbLink.Url = "Facebook";
            fbLink.Image = "Images/facebook.png";
            links.Add(fbLink);

            var twitterLink = new LinksData();
            twitterLink.Text = "Tweet";
            twitterLink.Url = "Twitter";
            twitterLink.Image = "Images/twitter.png";
            links.Add(twitterLink);


            MoreInfos.Links = links;

            NotifyPropertyChanged("MoreInfos");

        }

        public void SetNonUSDetail()
        {

            if (SelectedLocation == null)
            {
                return;
            }

            SelectedLocation.ReviewNumber = SelectedLocation.Reviews.Count().ToString();
            Reviews = new ObservableCollection<ReviewData>();
            foreach (var review in SelectedLocation.Reviews)
            {
                var reviewData = new ReviewData();
                reviewData.ReviewOwner = review.UserName;
                reviewData.ReviewDate = string.Format("{0:mm/dd/yyyy}", Convert.ToDateTime(review.Date));
                reviewData.ReviewText = review.Excerpt.Length > 135 ? review.Excerpt.Substring(0, 130) + "...." : review.Excerpt;
                reviewData.RatingValue = review.Rating;
                reviewData.CitySearchUrl = review.MobileUrl.AbsoluteUri;
                reviewData.ReviewOwnerPhotoUrl = review.UserPhotoUrl.AbsoluteUri;
                Reviews.Add(reviewData);

            }
            MoreInfos = new MoreInfoData();

            MoreInfos.Categories = String.Join(",", SelectedLocation.Categories.Select(n => n.Name).ToArray());

            var links = new List<LinksData>();

            var webSiteUrls = SelectedLocation.MobileWebSiteUrl;

            if (!string.IsNullOrEmpty(webSiteUrls))
            {
                var linkData = new LinksData();
                linkData.Text = "Website";
                linkData.Url = webSiteUrls;
                linkData.Image = "Images/Website.png";
                links.Add(linkData);
            }

            var phone = SelectedLocation.Phone;

            if (!string.IsNullOrEmpty(phone))
            {
                var linkData = new LinksData();
                linkData.Text = "Phone";
                linkData.Url = "Phone";
                linkData.Image = "Images/Phone.png";
                links.Add(linkData);
            }


            var address = SelectedLocation.Address;

            if (!string.IsNullOrEmpty(address))
            {
                var linkData = new LinksData();
                linkData.Text = "Map";
                linkData.Url = "Map";
                linkData.Image = "Images/Address.png";
                links.Add(linkData);
            }

            var fbLink = new LinksData();
            fbLink.Text = "Post to wall";
            fbLink.Url = "Facebook";
            fbLink.Image = "Images/facebook.png";
            links.Add(fbLink);

            var twitterLink = new LinksData();
            twitterLink.Text = "Tweet";
            twitterLink.Url = "Twitter";
            twitterLink.Image = "Images/twitter.png";
            links.Add(twitterLink);

            MoreInfos.Links = links;
            NotifyPropertyChanged("MoreInfos");

            NotifyPropertyChanged("Reviews");
        }

        public void SetDetail(string data)
        {
            var doc = XDocument.Parse(data);
            Reviews = new ObservableCollection<ReviewData>();
            var totalreviews = from location in doc.Descendants("reviews")
                               from review in location.Elements("review")
                               select new
                               {
                                   Name = (string)review.Element("review_author"),
                                   Date = (string)review.Element("review_date"),
                                   Text = (string)review.Element("review_text"),
                                   Rating = (string)review.Element("review_rating"),
                                   CitySearchUrl = (string)review.Element("review_url")
                               };


            SelectedLocation.ReviewNumber = totalreviews.Count().ToString();
            foreach (var review in totalreviews)
            {
                var reviewData = new ReviewData();
                reviewData.ReviewOwner = review.Name;
                reviewData.ReviewDate = string.Format("{0:mm/dd/yyyy}", Convert.ToDateTime(review.Date));
                reviewData.ReviewText = review.Text.Length > 135 ? review.Text.Substring(0, 130) + "...." : review.Text;
                double rating;
                bool isNum = Double.TryParse(review.Rating, out rating);
                if (isNum)
                {
                    reviewData.RatingValue = string.IsNullOrEmpty(review.Rating)
                                                 ? 0
                                                 : rating / 10;
                }
                else
                {
                    reviewData.RatingValue = 0;
                }
                reviewData.CitySearchUrl = review.CitySearchUrl;
                Reviews.Add(reviewData);

            }

            NotifyPropertyChanged("Reviews");

            var phone =
            from contactInfo in doc.Descendants("contact_info")
            select (string)contactInfo.Element("display_phone");

            if (phone.FirstOrDefault() != null)
            {
                SelectedLocation.Phone = phone.FirstOrDefault();
            }
            else
            {
                SelectedLocation.Phone = string.Empty;
            }
            NotifyPropertyChanged("SelectedLocation");
            HasPhoneNumber = SelectedLocation.Phone == string.Empty ? Visibility.Collapsed : Visibility.Visible;
            NotifyPropertyChanged("HasPhoneNumber");
            Editorials = new ObservableCollection<EditorialData>();
            var editorials = from location in doc.Descendants("editorials")
                             from editorial in location.Elements("editorial")
                             select new
                             {
                                 Title = (string)editorial.Element("editorial_title"),
                                 Author = (string)editorial.Element("editorial_author"),
                                 Date = (string)editorial.Element("editorial_date"),
                                 Text = (string)editorial.Element("editorial_review"),
                                 Rating = (string)editorial.Element("review_rating"),
                                 CitySearchUrl = (string)editorial.Element("editorial_url")
                             };

            foreach (var editorial in editorials)
            {
                var editorialData = new EditorialData();
                editorialData.Title = editorial.Title;
                editorialData.Author = editorial.Author;
                editorialData.ReviewDate = string.Format("{0:mm/dd/yyyy}", Convert.ToDateTime(editorial.Date));
                editorialData.ReviewText = StripHtmlTags(editorial.Text);
                double rating;
                bool isNum = Double.TryParse(editorial.Rating, out rating);
                if (isNum)
                {
                    editorialData.RatingValue = string.IsNullOrEmpty(editorial.Rating)
                                                 ? 0
                                                 : rating / 10;
                }
                else
                {
                    editorialData.RatingValue = 0;
                }
                editorialData.CitySearchUrl = editorial.CitySearchUrl;
                Editorials.Add(editorialData);

            }

            NotifyPropertyChanged("Editorials");
            SetMoreInfoData(data);
        }

        private string StripHtmlTags(string value)
        {
            int length = 0;
            int.TryParse(value, out length);
            string formattedValue = Regex.Replace(value as string, "<.*?>", "");
            formattedValue = Regex.Replace(formattedValue, @"\n+\s+", "\n\n");
            formattedValue = formattedValue.TrimStart(' ');
            formattedValue = HttpUtility.HtmlDecode(formattedValue);
            if (length > 0 && formattedValue.Length >= length)
                formattedValue = formattedValue.Substring(0, length - 1);
            return formattedValue;
        }


        public List<string> Metros { get; set; }

        public void SetMetroData()
        {
            if (Metros == null)
            {
                Metros = new List<string>();
            }

            if (Metros.Count > 0)
            {
                return;
            }
            var doc = XDocument.Load("Metros.XML");
            var metros = from location in doc.Descendants("METRO")
                         select ((string)location.Attribute("Name")).Trim().Substring(0,
                         ((string)location.Attribute("Name")).Length - 2);

            var listMetros = metros.OrderBy(x => x).Distinct();
            foreach (var metro in listMetros)
            {
                Metros.Add(metro);
            }

            _metrosCollection.Source = Metros;
            NotifyPropertyChanged("Metros");
        }


        private IsolatedStorageSettings _userSettings =
        IsolatedStorageSettings.ApplicationSettings;

        public ObservableCollection<string> StoredSearchLocations { get; set; }
        public ObservableCollection<string> StoredSearchLocationsView
        {
            get
            {
                var o = new ObservableCollection<string>();
                StoredSearchLocations.Take(10).ToList().ForEach(o.Add);
                return o;
            }
        }

        public void LoadFavourites()
        {
            try
            {
                if (_userSettings.Contains("Favourites"))
                {
                    var storedFavourites = (ObservableCollection<ListFavourite>)_userSettings["Favourites"];
                    foreach (var storedFavourite in storedFavourites)
                    {
                        Favourites.Add(storedFavourite);
                    }
                }
                else
                {
                    int counter = 0;
                    Favourites.Add(new ListFavourite { Code = "Amusement Parks", Counter = (++counter).ToString() + "." });
                    Favourites.Add(new ListFavourite { Code = "Golf", Counter = (++counter).ToString() + "." });
                    Favourites.Add(new ListFavourite { Code = "Hiking", Counter = (++counter).ToString() + "." });
                    _userSettings.Add("Favourites", Favourites);
                    _userSettings.Save();
                }

                NotifyPropertyChanged("Favourites");
                NotifyPropertyChanged("ListFavourites");
            }
            catch
            {

            }
        }



        public void LoadStoredSearchLocations()
        {
            try
            {
                if (_userSettings.Contains("HotSpotsSearchLocation"))
                {
                    var storedSearchLocations = (ObservableCollection<string>)_userSettings["HotSpotsSearchLocation"];
                    foreach (var storedSearchLocation in storedSearchLocations)
                    {
                        StoredSearchLocations.Add(storedSearchLocation);
                    }
                }
                else
                {
                    StoredSearchLocations.Add("Current Location");
                    _userSettings.Add("HotSpotsSearchLocation", StoredSearchLocations);
                    _userSettings.Save();
                }

                NotifyPropertyChanged("StoredSearchLocations");
                NotifyPropertyChanged("StoredSearchLocationsView");
            }
            catch
            {

            }

        }

        public void SaveSearchedLocations(string text)
        {
            if (text.ToLower() == "current location")
            {
                return;
            }
            if (String.IsNullOrEmpty(text.Trim()))
            {
                return;
            }
            var item = StoredSearchLocations.Where(s => s.ToLower() == text.ToLower()).FirstOrDefault();
            if (!StoredSearchLocations.Contains(item))
            {
                StoredSearchLocations.Add(text);
            }
            _userSettings["HotSpotsSearchLocation"] = StoredSearchLocations;
            _userSettings.Save();
            NotifyPropertyChanged("StoredSearchLocations");
            NotifyPropertyChanged("StoredSearchLocationsView");
        }

        public ObservableCollection<ListFavourite> Favourites { get; set; }
        public ListFavourite SelectedFavourite
        {
            get;
            set;
        }

        private void ReArrangeFavouritesCounter()
        {
            var counter = 0;
            foreach (var item in Favourites)
            {
                item.Counter = (++counter).ToString() + ".";

            }
        }

        public void AddFavourite(string faveText)
        {
            if (String.IsNullOrEmpty(faveText.Trim()))
            {
                return;
            }

            var item = Favourites.Where(s => s.Code.ToLower() == faveText.ToLower()).FirstOrDefault();
            if (!Favourites.Contains(item))
            {
                Favourites.Add(new ListFavourite { Code = faveText });
                ReArrangeFavouritesCounter();
                _userSettings["Favourites"] = Favourites;
                _userSettings.Save();
            }

            NotifyPropertyChanged("Favourites");
        }

        public void DeleteFavourites()
        {
            for (int i = Favourites.Count - 1; i >= 0; i--)
            {
                if (Favourites[i].IsSelected)
                {
                    Favourites.RemoveAt(i);
                }
            }

            ReArrangeFavouritesCounter();
            NotifyPropertyChanged("Favourites");
            _userSettings["Favourites"] = Favourites;
            _userSettings.Save(); ;

        }
    }




}
