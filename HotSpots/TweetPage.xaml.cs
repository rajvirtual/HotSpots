using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using HotSpots.Twitter;
using Microsoft.Phone.Shell;

namespace HotSpots
{
    public partial class TweetPage
    {
        private TwitterAccess _twitterSettings;

        public TweetPage()
        {
            InitializeComponent();
            CreateApplicationBar();

            UpdateRemainingCharacters();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _twitterSettings = Helper.LoadSetting<TwitterAccess>(Constants.TwitterAccess);
            if (_twitterSettings == null) return;

            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = !String.IsNullOrEmpty(_twitterSettings.AccessToken) && !String.IsNullOrEmpty(_twitterSettings.AccessTokenSecret);

            var detailItem = Helper.LoadSetting<TweetPageData>(Constants.TweetPageFileName);
            if (detailItem != null)
            {
                TweetTextBox.Text = detailItem.Tweet;
            }
        }

        #region ApplicationBar

        private void CreateApplicationBar()
        {
            ApplicationBar = new ApplicationBar { IsMenuEnabled = true, IsVisible = true, Opacity = .9 };

            var save = new ApplicationBarIconButton(new Uri("/Images/Ok.png", UriKind.Relative));
            save.Text = "Tweet";
            save.Click += SaveClick;
            save.IsEnabled = false;

            var cancel = new ApplicationBarIconButton(new Uri("/Images/Cancel.png", UriKind.Relative));
            cancel.Text = "Cancel";
            cancel.Click += CancelClick;

            ApplicationBar.Buttons.Add(save);
            ApplicationBar.Buttons.Add(cancel);
        }

        private void CancelClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Details.xaml", UriKind.RelativeOrAbsolute));
        }

        private void SaveClick(object sender, EventArgs e)
        {
            PostTweet();
        }

        #endregion

        private void UpdateRemainingCharacters()
        {
            CharactersCountTextBlock.Text = String.Format("{0} characters remaining", 140 - TweetTextBox.Text.Length);
        }

        private void TweetTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateRemainingCharacters();
        }

        private void MessageTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Focus();
            }
        }

        private void PostTweet()
        {
            if (String.IsNullOrEmpty(TweetTextBox.Text))
                return;

            ProgressBar.Visibility = Visibility.Visible;
            ProgressBar.IsIndeterminate = true;

            var twitter = new TwitterHelper();

            // Successful event handler, navigate back if successful
            twitter.TweetCompletedEvent += (sender, e) =>
                {
                    ProgressBar.Visibility = Visibility.Collapsed;
                    ProgressBar.IsIndeterminate = false;
                    MessageBox.Show("Message posted to Twitter");
                    NavigationService.Navigate(new Uri("/Details.xaml",
                                                       UriKind.RelativeOrAbsolute));
                };

            // Failed event handler, show error
            twitter.ErrorEvent += (sender, e) =>
                {
                    ProgressBar.Visibility = Visibility.Collapsed;
                    ProgressBar.IsIndeterminate = false;
                    MessageBox.Show("There was an error connecting to Twitter");
                };

            twitter.NewTweet(TweetTextBox.Text);
        }

        private void TweetTextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

    }
}