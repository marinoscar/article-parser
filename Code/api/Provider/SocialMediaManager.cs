using api.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace api.Provider
{
    public class SocialMediaManager
    {
        public IRestResponse Publish(PublishSocialMediaOptions options)
        {
            var profile = GetProfile();
            var url = string.Format("{0}/updates/create.json?access_token={1}", ConfigurationManager.AppSettings["buffer.api"], profile.Token);
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            var payload = GetPayload(options, profile);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", payload, ParameterType.RequestBody);
            return client.Execute(request);
        }

        private string GetPayload(PublishSocialMediaOptions options, Profile profile)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("text={0}\n{1}&", HttpUtility.HtmlEncode(options.Text), HttpUtility.HtmlEncode(options.Url));
            if (options.DoFacebook) sb.AppendFormat("profile_ids%5B%5D={0}&", profile.Fb);
            if (options.DoLinkedIn) sb.AppendFormat("profile_ids%5B%5D={0}&", profile.Li);
            if (options.DoTwitter) sb.AppendFormat("profile_ids%5B%5D={0}&", profile.Tw);
            if(!options.Buffer)
                sb.AppendFormat("now={0}", !options.Buffer);
            return sb.ToString();
        }

        private Profile GetProfile()
        {
            return new Profile()
            {
                Token = ConfigurationManager.AppSettings["buffer.token"],
                Fb = ConfigurationManager.AppSettings["buffer.fb"],
                Tw = ConfigurationManager.AppSettings["buffer.tw"],
                Li = ConfigurationManager.AppSettings["buffer.li"],
            };
        }

        private struct Profile
        {
            public string Token;
            public string Fb;
            public string Tw;
            public string Li;
        }
    }
}
