namespace HotSpots
{
    public enum SearchLocationType
    {
        Local,
        Other
    } ;

    public enum SearchCriteriaType
    {
        Type,
        What
    } ;

    public enum NonUSSearchCriteriaType
    {
        Term,
        Category
    } ;


    public class SearchCriteria
    {
        public SearchLocationType LocationType { get; set; }
        public string SearchLocation { get; set; }
        public string SearchText { get; set; }
        public string NonUSSearchText { get; set; }
        public SearchCriteriaType CriteriaType { get; set; }
        public NonUSSearchCriteriaType NonUSCriteriaType { get; set; }
        public string LocationLat { get; set; }
        public string LocationLong { get; set; }

        private string GetHasSearchOffersUrl()
        {
            var result = string.Empty;
            if (App.DataViewModel.SearchForOffers)
            {
                return "&has_offers=true";
            }

            return result;
        }

        private string BuildNonUSSearchListUrl()
        {
            var url = string.Empty;
            var viewModel = App.DataViewModel;
            url = "http://api.yelp.com/business_review_search?limit=20&ywsid=T28tTWv4HKwCQP2pVCetOg";
            if (LocationType == SearchLocationType.Local)
            {
                if (NonUSCriteriaType == NonUSSearchCriteriaType.Category)
                {
                    url = url + "&category=" + NonUSSearchText + "&lat=" + LocationLat + "&long=" + LocationLong;
                }
                else
                {
                    url = url + "&term=" + NonUSSearchText + "&lat=" + LocationLat + "&long=" + LocationLong;
                }
            }
            else{
                if (NonUSCriteriaType == NonUSSearchCriteriaType.Category)
                {
                    url = url + "&category=" + NonUSSearchText + "&location=" + SearchLocation;
                }
                else
                {
                    url = url + "&term=" + NonUSSearchText + "&location=" + SearchLocation;
                }
            }
            return url;
        }

        public virtual string BuildSearchListUrl()
        {
            if (!App.DataViewModel.USLocation){
                return BuildNonUSSearchListUrl();
            }
            var url = string.Empty;
            var viewModel = App.DataViewModel;
            if (LocationType == SearchLocationType.Local)
            {
                if (CriteriaType == SearchCriteriaType.Type)
                {
                    url = "http://api.citygridmedia.com/content/places/v2/search/latlon?type=" +
                          SearchText + "&lat=" +
                          LocationLat + "&lon=" + LocationLong +
                          "&publisher=8999798002&api_key=knr53dcpeug8psty7367br94&rpp=" +
                          viewModel.GetSelectedRppType() + "&sort=" +
                          viewModel.GetSelectedSortType() +
                          GetHasSearchOffersUrl().Trim();
                }
                else
                {
                    url = "http://api.citygridmedia.com/content/places/v2/search/latlon?what=" +
                    SearchText + "&lat=" +
                    LocationLat + "&lon=" + LocationLong + "&publisher=8999798002&api_key=knr53dcpeug8psty7367br94&rpp=" +
                          viewModel.GetSelectedRppType() + "&sort=" +
                          viewModel.GetSelectedSortType() +
                          GetHasSearchOffersUrl().Trim();
                }
            }
            else
            {
                if (CriteriaType == SearchCriteriaType.Type)
                {
                    url =
                        "http://api.citygridmedia.com/content/places/v2/search/where?type=" + SearchText +
                        "&where=" +
                        SearchLocation + "&publisher=8999798002&api_key=knr53dcpeug8psty7367br94" + "&rpp=" +
                          viewModel.GetSelectedRppType() + "&sort=" +
                          viewModel.GetSelectedSortType() +
                          GetHasSearchOffersUrl().Trim();
                }
                else
                {
                    url =
                           "http://api.citygridmedia.com/content/places/v2/search/where?what=" + SearchText +
                           "&where=" +
                           SearchLocation + "&publisher=8999798002&api_key=knr53dcpeug8psty7367br94" + "&rpp="
+
                          viewModel.GetSelectedRppType() + "&sort=" +
                          viewModel.GetSelectedSortType() +
                          GetHasSearchOffersUrl().Trim();
                }
            }

            return url;
        }

    }

}
