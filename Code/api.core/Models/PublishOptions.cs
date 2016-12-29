using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.core.Models
{
    public class PublishOptions
    {
        public ParseOptions Parse { get; set; }
        public WordpressOption Article { get; set; }
        public PublishSocialMediaOptions SocialMedia { get; set; }
    }

    public class PublishSocialMediaOptions
    {
        public string Url { get; set; }
        public string Text { get; set; }
        public bool DoFacebook { get; set;}
        public bool DoTwitter { get; set; }
        public bool DoLinkedIn { get; set; }
        public bool Buffer { get; set; }
    }
}
