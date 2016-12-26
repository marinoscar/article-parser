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

        private IRestResponse Execute(string url, string apiCall)
        {
            var client = new RestClient(GetUrl());
            var request = new RestRequest(apiCall, Method.POST);
            request.AddParameter("apikey", GetApiKey());
            request.AddParameter("application/x-www-form-urlencoded", string.Format("outputMode=json&url={0}", url), ParameterType.RequestBody);
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
            foreach(JToken token in array)
            {
                result.Add(new Keyword() {
                    Relevance = Convert.ToDouble(token["relevance"]),
                    Text = Convert.ToString(token["text"])
                });
            }
            return result;
        }

        private string GetApiKey()
        {
            return ConfigurationManager.AppSettings["watson.api.key"];
        }

        private string GetUrl()
        {
            return ConfigurationManager.AppSettings["watson.api.url"];
        }
    }
}
