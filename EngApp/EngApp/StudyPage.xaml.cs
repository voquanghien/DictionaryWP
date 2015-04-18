using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.IO.IsolatedStorage;
using EngApp.Models;
using Windows.Phone.Speech.Recognition;
using Windows.Phone.Speech;
using Windows.Phone.Speech.Synthesis;

namespace EngApp
{
    public partial class StudyPage : PhoneApplicationPage
    {
        private Deck Deck;
        ProgressIndicator prog;
        public bool edit = false;
        string deckName = "";
        public StudyPage()
        {
            InitializeComponent();
            prog = new ProgressIndicator();
            SystemTray.SetOpacity(this, 0.5);
            SystemTray.SetProgressIndicator(this, prog);
            prog.IsIndeterminate = true;
            prog.Text = "Creating category...";
            
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Deck deck = null;
            if (e.NavigationMode == NavigationMode.Back)
            {
                deck = new Deck(settings["StudyDeckName"] as string);
            }
            else
            {
                string name = "";
                if (NavigationContext.QueryString.TryGetValue("DeckName", out name))
                {
                    deck = new Deck(name);

                }
            }
            SystemTray.SetIsVisible(this, true);
            prog.IsVisible = true;
            await deck.TryGetDeck();
            Pan.DataContext = deck;
            SystemTray.SetIsVisible(this, false);
            prog.IsVisible = false;
            deckName = deck.Name;

            Pan.SelectedItem = Pan.Items.Count == 0 ? null : Pan.Items[0];

        }
        private IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            settings["StudyDeckName"] = deckName;
        }

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            Grid grid = sender as Grid;
            if ((grid.FindName("WordViewBox") as Grid).Visibility == System.Windows.Visibility.Visible)
            {
                (grid.FindName("FlipAnimation") as Storyboard).Begin();
                (grid.FindName("VisibilityAnimation") as Storyboard).Begin();
            }
            else
            {
                (grid.FindName("FlipBackAnimation") as Storyboard).Begin();
                (grid.FindName("VisibilityBackAnimation") as Storyboard).Begin();
            }

        }

        private async void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            if (Pan.Items.Count > 0)
            {
                var f = Pan.SelectedItem as FlashCard;
                var d = Pan.DataContext as Deck;
                switch ((sender as ApplicationBarIconButton).Text)
                {
                    case "edit":
                        NavigationService.Navigate(new Uri("/EditCard.xaml?deck=" + d.Name + "&card=" + f.Word, UriKind.Relative));

                        break;
                    case "delete":

                        await Deck.DeleteCard(d.Name, f.Word);
                        Pan.DataContext = await d.TryGetDeck();
                        break;
                    case "DelCat":
                        await Deck.Delete(d.Name);
                        NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                        break;
                }
            }

        }

        

        private async void ItemGrid_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            var temp1 = Pan.SelectedItem as FlashCard;
            var temp2 = temp1.Word;
            await synth.SpeakTextAsync(temp2);
        }

       
    }
}