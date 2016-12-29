using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.core.Models
{
    public class WatsonResult<T>
    {

        public WatsonResult()
        {
            Items = new List<T>();
        }
        public string Status { get; set; }
        public string Language { get; set; }
        public IList<T> Items { get; set; }
    }
}
