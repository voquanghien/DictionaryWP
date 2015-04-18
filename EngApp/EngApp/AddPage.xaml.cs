using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using EngApp.Models;

namespace EngApp
{
    public partial class AddPage : PhoneApplicationPage
    {
        ProgressIndicator prog;
        bool success = false;
        public AddPage()
        {
            InitializeComponent();
            prog = new ProgressIndicator();
            SystemTray.SetOpacity(this, 0.5);
            SystemTray.SetProgressIndicator(this, prog);
            prog.IsIndeterminate = true;
            prog.Text = "creating deck...";
        }
        private IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        private async void OnCheckButtonClick(object sender, EventArgs e)
        {
            success = false;
            string deckName = NameTextBox.Text.Trim();
            deckName = deckName == "" ? null : deckName;

            if (deckName == null)
            {
                MessageBox.Show("Deck name can not be empty");
                return;
            }
            if (!await Deck.CheckNameCollision(deckName))
            {
                if (MessageBox.Show("The name of the deck already exists. The new Flashcards will be appended to the existing Deck", "Are you sure", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                    return;

            }

            String words = WordsTextBox.Text.Trim();
            words = words == "" ? null : words;

            if (words == null)
            {
                MessageBox.Show("Can't create an empty deck.");
                return;
            }

            string[] s = words.Split(" ".ToCharArray());
            Deck deck = new Deck(deckName);
            foreach (string ss in s)
            {
                try
                {
                    FlashCard fc = await Dictionary.Lookup(ss);
                    deck.FlashCards.Add(fc);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Something went wrong. Check your Internet connection and try again please");
                    return;
                }

            }
            success = true;
            NavigationService.GoBack();
            BeginWork();
            await deck.Write();
            EndWork();

        }

        private void BeginWork()
        {
            SystemTray.SetIsVisible(this, true);
            prog.IsVisible = true;
        }

        private void EndWork()
        {
            prog.IsVisible = false;
            SystemTray.SetIsVisible(this, false);
        }

        private void NameTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                WordsTextBox.Focus();
        }

        private void WordsTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                wtf.Focus();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (e.NavigationMode == NavigationMode.Back ||
                e.NavigationMode == NavigationMode.New)
            {
                try
                {
                    WordsTextBox.Text = settings["words"] as string;
                    NameTextBox.Text = settings["name"] as string;

                }
                catch (Exception) //if they dont exist do nothing.
                {

                }
            }

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (!success)
            {
                settings["words"] = WordsTextBox.Text;
                settings["name"] = NameTextBox.Text;
            }
            else
            {
                settings["words"] = "";
                settings["name"] = "";
            }
        }
    }
}