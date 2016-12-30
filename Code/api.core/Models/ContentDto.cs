using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.core.Models
{
    public class ContentDto
    {
        public string Id { get; set; }
        public string WordpressId { get; set; }
        public string Title { get; set; }
        public DateTime UtcUpdatedOn { get; set; }
    }
}
