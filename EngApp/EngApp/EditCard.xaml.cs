using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Storage;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Media;
using System.Collections.ObjectModel;
using EngApp.Models;

namespace EngApp
{
    public partial class Page1 : PhoneApplicationPage
    {
        string deckName, cardName;
        List<PhoneTextBox> boxes = new List<PhoneTextBox>();
        public Page1()
        {
            InitializeComponent();

        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            deckName = NavigationContext.QueryString["deck"];
            cardName = NavigationContext.QueryString["card"];
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("decks", CreationCollisionOption.OpenIfExists);
            var stream = await (await (await folder.GetFolderAsync(deckName)).GetFileAsync(cardName)).OpenStreamForReadAsync();

            using (StreamReader reader = new StreamReader(stream))
            {
                string s = await reader.ReadToEndAsync();
                FlashCard f = JsonConvert.DeserializeObject<FlashCard>(s);
                panel.DataContext = f;
                foreach (var v in f.Definitions)
                {
                    PhoneTextBox box = new PhoneTextBox();
                    box.Style = this.Resources["temp"] as Style;
                    box.Text = v;
                    box.TextWrapping = TextWrapping.Wrap;
                    boxes.Add(box);
                    list.Children.Add(box);
                }
            }

        }

        private async void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("decks", CreationCollisionOption.OpenIfExists);
            folder = await folder.GetFolderAsync(deckName);
            ObservableCollection<string> ss = new ObservableCollection<string>();
            foreach (var v in boxes)
            {
                ss.Add(v.Text);

            }

            FlashCard f = new FlashCard(cardName, ss);
            string fs = JsonConvert.SerializeObject(f);
            var file = await folder.CreateFileAsync(cardName, CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(fs.ToCharArray());
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }

            NavigationService.GoBack();

        }


    }
}