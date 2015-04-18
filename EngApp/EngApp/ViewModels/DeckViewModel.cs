using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using EngApp.Models;

namespace EngApp.ViewModels
{
    public class DeckViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Deck> decks;
        public ObservableCollection<Deck> Decks
        {
            get
            {
                return decks;
            }
            set
            {
                if (decks != value)
                {
                    decks = value;
                    onPropertyChanged();
                }
            }
        }

        private DateTime dateTime;

        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }


        public DeckViewModel()
        {
            load();
        }

        public async void load()
        {
            Decks = await LoadDecks();
        }
        //mainpage uses it to load the decks when navigated to
        public async static Task<ObservableCollection<Deck>> LoadDecks()
        {
            ObservableCollection<Deck> decks = new ObservableCollection<Deck>();
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("decks", CreationCollisionOption.OpenIfExists);
            var folders = await folder.GetFoldersAsync();
            foreach (StorageFolder f in folders)
            {
                decks.Add(new Deck(f.Name));
            }
            return decks;
        }
        private void onPropertyChanged([CallerMemberName] string s = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(s));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
