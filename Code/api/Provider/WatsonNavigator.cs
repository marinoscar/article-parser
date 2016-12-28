using api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class WatsonNavigator
    {

        public WatsonResult<Keyword> GetKeywords(string url)
        {
            var response = Execute(url, "URLGetRankedKeywords");
            var json = ParseResponse(response);
            var result = LoadResponse<Keyword>(json, LoadKeywords);
            return result;
        }

        public WatsonResult<string> GetTaxonomy(string url)
        {
            var response = Execute(url, "URLGetRankedTaxonomy");
            var json = ParseResponse(response);
            var result = LoadResponse<string>(json, LoadCategories);
            return result;
        }

        private IRestResponse Execute(string url, string apiCall)
        {
            var apiUrl = string.Format("{0}/{1}?apikey={2}", GetUrl(), apiCall, GetApiKey());
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", string.Format("outputMode=json&url={0}", HttpUtility.UrlEncode(url)), ParameterType.RequestBody);
            return client.Execute(request);
        }

        private JToken ParseResponse(IRestResponse response)
        {
            return (JToken)JsonConvert.DeserializeObject(response.Content);
        }

        private WatsonResult<T> LoadResponse<T>(JToken json, Func<JToken, List<T>> itemLoad)
        {
            var result = new WatsonResult<T>() {
                Status = Convert.ToString(json["status"]),
                Language = Convert.ToString(json["language"]),
                Items = itemLoad(json)
            };
            return result;
        }

        private List<Keyword> LoadKeywords(JToken json)
        {
            var result = new List<Keyword>();
            var array = (JArray)json["keywords"];
            if (array == null) return result;
            foreach(JToken token in array)
            {
                result.Add(new Keyword() {
                    Relevance = Convert.ToDouble(token["relevance"]),
                    Text = Convert.ToString(token["text"])
                });
            }
            return result;
        }

        private List<string> LoadCategories(JToken json)
        {
            var result = new List<TaxonomyItem>();
            var array = (JArray)json["taxonomy"];
            if (array == null) return new List<string>();
            foreach (JToken token in array)
            {
                result.Add(new TaxonomyItem()
                {
                    Score = Convert.ToDouble(token["score"]),
                    Label = Convert.ToString(token["label"]),
                    IsConfident = token["confident"] == null
                });
            }
            var item = result.Where(i => i.IsConfident).OrderByDescending(i => i.Score).FirstOrDefault();
            return item == null ? new List<string>() : item.Label.Split("/".ToCharArray()).Where(i => !string.IsNullOrWhiteSpace(i)).ToList();
        }

        private string GetApiKey()
        {
            return ConfigurationManager.AppSettings["watson.api.key"];
        }

        private Uri GetUrl()
        {
            return new Uri(ConfigurationManager.AppSettings["watson.api.url"]);
        }
    }
}
