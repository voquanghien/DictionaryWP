using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Storage;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Microsoft.Phone.Tasks;

namespace EngApp.Models
{
    public class Deck : INotifyPropertyChanged
    {
        private int count;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                if (value != count)
                {
                    count = value;
                    onPropertyChanged();
                }


            }
        }

        private int color;

        public int Color
        {
            get { return color; }
            set { color = value; }
        }


        private void onPropertyChanged([CallerMemberName] string s = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(s));
            }
        }

        private DateTime dateModified;

        public DateTime DateModified
        {
            get { return dateModified; }
            set { dateModified = value; onPropertyChanged(); }
        }


        public string Name { get; set; }
        ObservableCollection<FlashCard> flashCards;
        public ObservableCollection<FlashCard> FlashCards
        {
            get
            {
                if (flashCards == null) flashCards = new ObservableCollection<FlashCard>();
                return flashCards;
            }
            set
            {
                flashCards = value;
                Count = flashCards.Count;
                onPropertyChanged();
            }
        }

        static int i = 0;

        public Deck(string name)
        {
            Name = name;
            DateModified = DateTime.Now;
            color = i++ % 5;

        }
        //this method is used to deserialize all the flashcards for the studypage
        public async Task<Deck> TryGetDeck()
        {
            FlashCards.Clear();
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("decks", CreationCollisionOption.OpenIfExists);
            var deckFolder = await folder.CreateFolderAsync(Name, CreationCollisionOption.OpenIfExists);
            var decks = await deckFolder.GetFilesAsync();
            foreach (var v in decks)
            {
                using (var stream = await v.OpenStreamForReadAsync())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string text = await reader.ReadToEndAsync();
                        FlashCard f = await JsonConvert.DeserializeObjectAsync<FlashCard>(text);
                        FlashCards.Add(f);
                    }
                }
            }

            return this;
        }

        public static async Task<bool> CheckNameCollision(string s)
        {
            StorageFolder folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("decks", CreationCollisionOption.OpenIfExists);
            try
            {
                folder = await folder.GetFolderAsync(s);
                return false;

            }
            catch (Exception e)
            {
                return true;
            }
        }
        public async static Task Delcat()
        {
            
        }
        public async static Task Delete(string name)
        {
            StorageFolder folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("decks", CreationCollisionOption.OpenIfExists);
            var f = await folder.GetFolderAsync(name);
            await f.DeleteAsync();
        }

        public async static Task DeleteCard(string dname, string fname)
        {
            StorageFolder folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("decks", CreationCollisionOption.OpenIfExists);
            var f = await folder.GetFolderAsync(dname);
            var file = await f.GetFileAsync(fname);
            await file.DeleteAsync();
        }

        public async Task DeleteCard(string fname)
        {
            StorageFolder folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("decks", CreationCollisionOption.OpenIfExists);
            var f = await folder.GetFolderAsync(Name);
            var file = await f.GetFileAsync(fname);
            await file.DeleteAsync();
        }


        public async Task Write()
        {
            try
            {
                StorageFolder folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("decks", CreationCollisionOption.OpenIfExists);
                folder = await folder.CreateFolderAsync(Name, CreationCollisionOption.OpenIfExists);
                foreach (var f in FlashCards)
                {
                    string s = await JsonConvert.SerializeObjectAsync(f);
                    byte[] stream = System.Text.Encoding.UTF8.GetBytes(s.ToCharArray());
                    StorageFile file = null;
                    try
                    {
                        file = await folder.CreateFileAsync(f.Word, CreationCollisionOption.FailIfExists);
                    }
                    catch (Exception) { continue; } // do nothing because don't want to erarase user's data.
                    using (var writeStream = await file.OpenStreamForWriteAsync())
                    {
                        writeStream.Write(stream, 0, stream.Length);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
      
    }
}
