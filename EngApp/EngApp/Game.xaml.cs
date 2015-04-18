using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;

namespace EngApp
{
    public partial class Game : PhoneApplicationPage
    {
        private string MainUri = "/Html/index.html";
        private string currentPage = "menu";
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;
        public Game()
        {
            InitializeComponent();
        }
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            // Cancel the back button press only if we are not in the menu
            if (currentPage != "menu")
            {
                Browser.InvokeScript("cellBackButtonPressed", currentPage);
                e.Cancel = true;
            }
        }

        private void loadHighscores()
        {

            //var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            //roamingSettings.Values["highscores"] = "[{a:'AMD',v:'12'},{a:'MON',v:'9'}]";
            try
            {
                string name = (string)userSettings["highscores"];
                Browser.InvokeScript("HighscoresLoaded", name);
            }
            catch (Exception ex)
            {
                Browser.InvokeScript("HighscoresLoaded", "");
            }

        }


        private void loadSettings()
        {

            try
            {
                string[] settings = new string[2];
                settings[0] = (string)userSettings["language"];
                settings[1] = (string)userSettings["music"];
                Browser.InvokeScript("SettingsLoaded", settings);
            }
            catch (Exception ex)
            {
                // language=en by default
                string[] settings = new string[2];
                settings[0] = "en";
                settings[1] = "off";
                saveLanguage("en"); saveMusic("off");
                Browser.InvokeScript("SettingsLoaded", settings);
            }

        }

        private void saveHighscores(string highscores)
        {
            if (userSettings.Contains("highscores"))
            {
                userSettings["highscores"] = highscores;
            }
            else
            {
                userSettings.Add("highscores", highscores);
            }
        }

        private void saveMusic(string music)
        {
            if (userSettings.Contains("music"))
            {
                userSettings["music"] = music;
            }
            else
            {
                userSettings.Add("music", music);
            }
        }

        private void saveLanguage(string language)
        {
            if (userSettings.Contains("language"))
            {
                userSettings["language"] = language;
            }
            else
            {
                userSettings.Add("language", language);
            }
        }

        private void HTML_Script_Launched(object sender, NotifyEventArgs e)
        {
            if (e.Value.StartsWith("frameChange="))
            {
                // change current page value
                currentPage = e.Value.Replace("frameChange=", "");

                if (currentPage == "resultsframe")
                {
                    // we display the publicity only if we are in the results
                    adView.Visibility = Visibility.Visible;
                }
                else
                {
                    // hide publicity in any other case
                    adView.Visibility = Visibility.Collapsed;
                }
            }

            if (e.Value.StartsWith("loadSettings"))
            {
                loadSettings();
            }

            if (e.Value.StartsWith("loadHighscores"))
            {
                loadHighscores();
            }

            if (e.Value.StartsWith("saveHighscores="))
            {
                saveHighscores(e.Value.Replace("saveHighscores=", ""));
            }

            if (e.Value.StartsWith("saveMusic="))
            {
                saveMusic(e.Value.Replace("saveMusic=", ""));
            }

            if (e.Value.StartsWith("saveLanguage="))
            {
                saveLanguage(e.Value.Replace("saveLanguage=", ""));
            }
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            Browser.IsScriptEnabled = true;

            // Add your URL here
            Browser.Navigate(new Uri(MainUri, UriKind.Relative));

            // Reposition publicity if screen is bigger 
            if (App.Current.Host.Content.ScaleFactor == 150)
            {
                adView.Margin = new Thickness(0, 778, 0, 0);
            }

            // hide publicity
            adView.Visibility = Visibility.Collapsed;
            Browser.Visibility = Visibility.Visible;
        }

        // Navigates back in the web browser's navigation stack, not the applications.
        private void BackApplicationBar_Click(object sender, EventArgs e)
        {
            Browser.GoBack();
        }

        // Navigates forward in the web browser's navigation stack, not the applications.
        private void ForwardApplicationBar_Click(object sender, EventArgs e)
        {
            Browser.GoForward();
        }

        // Navigates to the initial "home" page.
        private void HomeMenuItem_Click(object sender, EventArgs e)
        {
            Browser.Navigate(new Uri(MainUri, UriKind.Relative));
        }

        // Handle navigation failures.
        private void Browser_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            MessageBox.Show("Navigation to this page failed, check your internet connection");
        }

        private void Browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            
            System.Threading.Thread.Sleep(1000);
            SplashPanel.Visibility = Visibility.Collapsed;
            
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            string message = "Do you want to leave this game now?";
            string caption = "Warning!!!";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxResult result; ;
            result = MessageBox.Show(message, caption, button);
            if (result == MessageBoxResult.OK)
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            
        }
    
    }

}