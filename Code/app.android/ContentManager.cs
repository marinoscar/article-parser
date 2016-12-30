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
using app.android.Models;
using RestSharp;
using System.Net;
using Newtonsoft.Json;

namespace app.android
{
    public class ContentManager
    {
        public Activity Activity { get; private set; }

        public ContentManager(Activity activity)
        {
            Activity = activity;
        }

        public Tuple<bool, string> PostData(ArticlePublish model)
        {
            var payload = model.ToJson();
            //var token = Activity.GetToken();
            //var client = new RestClient(string.Format("{0}/publish/persist", GetRootUrl()));
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("token", token);
            //request.AddHeader("content-type", "application/json");
            //request.AddParameter("application/json", payload, ParameterType.RequestBody);
            //var response = client.Execute(request);
            var response = GetResponse("publish/persist", Method.POST, payload);
            return ParseResponse(response);
        }

        public IEnumerable<ContentDto> GetArticles()
        {
            var response = GetResponse("parser/getarticles", Method.GET, null);
            if (response.StatusCode != HttpStatusCode.OK) return new List<ContentDto>();
            return JsonConvert.DeserializeObject<IEnumerable<ContentDto>>(response.Content);
        }

        private IRestResponse GetResponse(string method, Method verb, string payload)
        {
            var token = Activity.GetToken();
            var client = new RestClient(string.Format("{0}/{1}", GetRootUrl(), method));
            var request = new RestRequest(Method.POST);
            request.AddHeader("token", token);
            request.AddHeader("content-type", "application/json");
            if(!string.IsNullOrWhiteSpace(payload))
                request.AddParameter("application/json", payload, ParameterType.RequestBody);
            return client.Execute(request);
        }

        private Tuple<bool, string> ParseResponse(IRestResponse res)
        {
            if (res.StatusCode != HttpStatusCode.Created) return new Tuple<bool, string>(false, "There was an error processing your request. Please make sure the url is correct and try again");
            return new Tuple<bool, string>(true, "The content has been stored and processed");
        }

        private string GetRootUrl()
        {
            return "http://k-app.azurewebsites.net/api";
        }
    }
}