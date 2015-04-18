using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngApp
{
    public static class AppConfig
    {

        public static String ID { get; set; }
        public static String Language { get; set; }
        public static String Link { get; set; }

        static AppConfig()
        {
            ID = "121c6";
            Language = "enzh";
            Link = "http://api.wordreference.com/" + ID + "/json/" + Language + "/";

        }

    }
}
