using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Models
{
    public class PublishResult
    {
        public string ParseId { get; set; }
        public string WordpressId { get; set; }
        public bool DidPublishInSocial { get; set;}
    }
}
