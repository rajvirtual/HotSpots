using Microsoft.Phone.Controls;

namespace HotSpots
{
    public partial class OffersPage : PhoneApplicationPage
    {
        public OffersPage()
        {
            InitializeComponent();
            DataContext = App.DataViewModel;
        }

        private void DismissOffers(object sender, GestureEventArgs e)
        {
            this.NavigationService.GoBack();
        }


        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            Utils.SetUpAcceleratorIfRequired();
            base.OnNavigatedTo(e);
        }
    }
}