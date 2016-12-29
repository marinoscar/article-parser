using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.core.Models
{
    public class ParseOptions
    {
        public string Url { get; set; }
        public bool ExcludeImageInContent { get; set;}
        public string ImageClass { get; set; }

    }
}
