using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Windows;

namespace EngApp.Models
{
    public class Definition : DependencyObject
    {


        public string PartOfSpeech
        {
            get { return (string)GetValue(PartOfSpeechProperty); }
            set { SetValue(PartOfSpeechProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PartOfSpeech.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PartOfSpeechProperty =
            DependencyProperty.Register("PartOfSpeech", typeof(string), typeof(Definition), new PropertyMetadata(0));


        public string Def
        {
            get { return (string)GetValue(DefProperty); }
            set { SetValue(DefProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Def.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefProperty =
            DependencyProperty.Register("Def",
            typeof(string),
            typeof(Definition),
            new PropertyMetadata(new PropertyChangedCallback(OnValueChanged)));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public Definition(String pos, string def)
        {
            PartOfSpeech = pos;
            Def = def;
        }
    }

    public class FlashCard
    {
        public string Word { get; set; }
        public ObservableCollection<string> Definitions { get; set; }

        public FlashCard(String word, ObservableCollection<string> defs)
        {
            this.Word = word;
            this.Definitions = defs;
            if (Definitions != null)
                Definitions.CollectionChanged += Definitions_CollectionChanged;


        }

        void Definitions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

    }

    
}
