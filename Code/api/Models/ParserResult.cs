using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Models
{
    public class ParserResult
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string DatePublish { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public IEnumerable<string> Keywords { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
}
