using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EngApp.Models
{
    public class Dictionary
    {
        private static string baseUrl = "http://api.wordnik.com/v4/word.json/";
        private static string quertString = "/definitions?limit=200&includeRelated=false&sourceDictionaries=wordnet,webster,century&useCanonical=true&includeTags=false&api_key=679ceb64413da6840000b0c271a036b85eeff535fe9bad167";
        private static HttpClient client = new HttpClient();
        public async static Task<FlashCard> Lookup(string word)
        {
            string result;
            int j = 0;
            do
            {

                try
                {
                    result = await client.GetStringAsync(baseUrl + word + quertString);
                    break;
                }
                catch (Exception e)
                {
                    if (j++ > 2) throw e;

                }
            } while (true);
            var json = JArray.Parse(result);
            if (json.Count == 0) return new FlashCard(word, new ObservableCollection<string>() { "no defintiion found" });
            var defs = json.Children();
            ObservableCollection<string> def = new ObservableCollection<string>();

            int i = 0;
            foreach (var d in defs)
            {
                if (i++ < 3) //for now, only want less than 3 definitions
                {
                    string s = d.Value<string>("partOfSpeech") + ". " + d.Value<string>("text");
                    def.Add(s);
                }
            }
            return new FlashCard(word, def);

        }
    }
}
