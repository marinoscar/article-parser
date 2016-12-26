using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Models
{
    public class WordpressConfig
    {
        public string Url { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public static WordpressConfig Load()
        {
            return new WordpressConfig()
            {
                Password = ConfigurationManager.AppSettings["wp.password"],
                User = ConfigurationManager.AppSettings["wp.user"],
                Url = ConfigurationManager.AppSettings["wp.url"]
            };
        }
    }
}
