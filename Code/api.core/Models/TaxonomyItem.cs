using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.core.Models
{
    public class TaxonomyItem
    {
        public bool IsConfident { get; set; }
        public double Score { get; set; }
        public string Label { get; set; }
    }
}
