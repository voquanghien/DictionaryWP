using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using EngApp.ViewModels;
using EngApp.Models;
using System.Collections.ObjectModel;
using Windows.Phone.Devices.Notification;
using System;
using Windows.Phone.UI.Core;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Windows.Storage;
using System.IO;
using Coding4Fun.Toolkit.Controls;
using System.Windows.Input;


namespace EngApp
{

    public partial class MainPage : PhoneApplicationPage
    {
        public FlashCardViewModel fvm { get; set; }
        public DeckViewModel dvm { get; set; }
        public MainPage()
        {
            InitializeComponent();
            fvm = new FlashCardViewModel();
            dvm = new DeckViewModel();
            DefinitionList.DataContext = fvm;
            DeckList.DataContext = dvm;
            listPicker.DataContext = dvm;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
                dvm.load();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if ((sender as TextBox).Text.Trim() != "")
                    fvm.getDef(InputTextBox.Text.Trim());
                DefinitionList.Focus();
            }
        }


        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddPage.xaml", UriKind.Relative));
        }

        private void DeckList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var deck = (sender as Grid).DataContext as Deck;
            //deck = await deck.TryGetDeck();
            if (deck != null) //if the user actually tap on an item not the list itself
            {
                NavigationService.Navigate(new Uri("/StudyPage.xaml?DeckName=" + deck.Name, UriKind.Relative));
            }
        }

        private void InputTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void AddToDeckButton_Click(object sender, RoutedEventArgs e)
        {
            string s = "";
            foreach (var v in dvm.Decks)
                s += v.Name + " ";
            string ss = "?decks=" + s + "&word=" + InputTextBox.Text;
            NavigationService.Navigate(new Uri("/PickDeck.xaml" + ss, UriKind.Relative));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (InputTextBox.Text.Trim() == "")
            {
                string message = "There is no Flashcard now!";
                string caption = "Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxResult result;
                result = MessageBox.Show(message, caption, button);
            }
            if (InputTextBox.Text.Trim() != "")
            {
                if (this.listPicker.Items.Count == 0)
                {
                    string message = "Please add a category first!";
                    string caption = "Error";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBox.Show(message, caption, button);
                    this.MainPanorama.DefaultItem = this.MainPanorama.Items[0];
                    return;
                }
                var name = (listPicker.SelectedItem as Deck).Name;
                Deck deck = new Deck(name);
                deck.FlashCards.Add((DefinitionList.DataContext as FlashCardViewModel).FlashCard);
                await deck.Write();
                ToastPrompt toast = new ToastPrompt();
                toast.Message = "Flashcard " + InputTextBox.Text.Trim() + " is added to Category "
                                            + name;
                toast.FontSize = 24;
                toast.Show();

            }

        }

        private async void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            await Deck.Delete((sender as MenuItem).CommandParameter as string);
            dvm.load();
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Game.xaml", UriKind.Relative));
        }


    }



}