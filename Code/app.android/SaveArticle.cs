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
using System.Threading;
using System.Threading.Tasks;

namespace app.android
{
    [Activity(Label = "@string/SaveArticleScreen")]
    public class SaveArticle : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SaveArticle);
            SetupEvents();
        }

        private void SetupEvents()
        {
            var publishBtn = FindViewById<Button>(Resource.Id.publishBtn);
            publishBtn.Click += PublishBtn_Click;
        }

        private void PublishBtn_Click(object sender, EventArgs e)
        {
            var result = default(Tuple<bool, string>);
            this.ExecuteLongTask(() =>
            {
                result = PostData();
            }, () =>
            {
                if (!result.Item1)
                    this.ShowDialogOk(GoToMain, "Error", result.Item2);
                else
                {
                    Toast.MakeText(this, result.Item2, ToastLength.Long).Show();
                    GoToMain();
                }
            }, "Procesing", "Please wait while we complete your request");
        }

        private void GoToMain()
        {
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        private Tuple<bool, string> PostData()
        {
            var payload = GetModel().ToJson();
            var token = this.GetToken();
            var client = new RestClient("http://k-app.azurewebsites.net/api/publish/persist");
            var request = new RestRequest(Method.POST);
            request.AddHeader("token", token);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", payload, ParameterType.RequestBody);
            var response = client.Execute(request);
            return ParseResponse(response);
        }

        private Tuple<bool, string> ParseResponse(IRestResponse res)
        {
            if (res.StatusCode != HttpStatusCode.Created) return new Tuple<bool, string>(false, "There was an error processing your request. Please make sure the url is correct and try again");
            return new Tuple<bool, string>(true, "The content has been stored and processed");
        }

        private ArticlePublish GetModel()
        {
            var urlTxt = FindViewById<EditText>(Resource.Id.editText1);
            var result = new ArticlePublish()
            {
                Url = urlTxt.Text,
                Wordpress = FindViewById<CheckBox>(Resource.Id.wpChk).Checked,
                Facebook = FindViewById<CheckBox>(Resource.Id.fbCheck).Checked,
                Twitter = FindViewById<CheckBox>(Resource.Id.twitterChk).Checked,
                LinkedIn = FindViewById<CheckBox>(Resource.Id.linkedInChk).Checked
            };
            return result;
        }

    }
}