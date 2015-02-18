using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.GamerServices;

namespace HotSpots
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
            DataContext = App.DataViewModel;
            Loaded += new RoutedEventHandler(Settings_Loaded);

        }

        void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            var data = Utils.GetLocationServiceData();
            toggleButton.IsChecked = (data == "Yes");
        }

        private void UseLocationDataCheckedClick(object sender, RoutedEventArgs e)
        {
            var data = "No";
            if (toggleButton.IsChecked.HasValue)
            {
                data = toggleButton.IsChecked.Value ? "Yes" : "No";
            }
            if (data == "No")
            {
                App.DataViewModel.ServiceLocatorInitialized = false;
                App.DataViewModel.AllowedServiceLocator = false;
            }

            Utils.SetLocationServiceData(data);

        }

        private void EmailClick(object sender, RoutedEventArgs e)
        {
            var task = new EmailComposeTask();
            task.To = "info@virtualbeans.com";
            task.Subject = "Suggestions/Questions regarding HotSpots";
            task.Show();
        }


        private void ShowClearWarning()
        {
            var viewModel = DataContext as ViewModel;
            var sb = new StringBuilder();
            var result = false;
            sb.Append("This action will clear the country from your app settings.If you search for places again, this app will automatically identify your current country.");
            sb.Append("Do you want to continue ?");
            Guide.BeginShowMessageBox("Clear your current country ?", sb.ToString(),
                    new List<string> { "Yes", "No" }, 0, MessageBoxIcon.Error,
                    asyncResult =>
                    {
                        int? returned = Guide.EndShowMessageBox(asyncResult);

                        if (returned.HasValue && returned.Value == 0)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                viewModel.CountryLocated = false;
                                viewModel.CountryName = string.Empty;
                                Utils.SetCountryName(string.Empty);
                            })
                            ;
                        }
                    }, null);

        }


        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ShowClearWarning();

        }
    }
}