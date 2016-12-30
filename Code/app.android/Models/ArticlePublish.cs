using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace app.android.Models
{
    public class ArticlePublish
    {
        public string Url { get; set; }
        public bool Wordpress { get; set; }
        public bool Facebook { get; set; }
        public bool Twitter { get; set; }
        public bool LinkedIn { get; set; }
        public bool Buffer { get; set; }
        public string Text { get; set; }

        public string ToJson()
        {
            var template = @"
[
	""Parse"":[
		""Url"": ""{0}""
	],
	""Article"":[
		""PostStatus"": ""{1}""
	],
	""SocialMedia"":[
		""DoFacebook"": {2},
		""DoTwitter"": {3},
		""DoLinkedIn"": {4},
		""Buffer"": {5},
        ""Text"":""{6}""
	]
]
";
            return string.Format(template, Url,
                Wordpress ? "publish" : "no",
                Facebook.ToString().ToLowerInvariant(), 
                Twitter.ToString().ToLowerInvariant(), 
                LinkedIn.ToString().ToLowerInvariant(),
                Buffer.ToString().ToLowerInvariant(),
                Text
                ).Replace("[", "{").Replace("]", "}");
        }
    }
}