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

namespace app.android
{
    [Activity(Label = "@string/SaveArticleScreen")]
    public class SaveArticle : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SaveArticle);
            
        }

        private void SetupEvents()
        {
            var publishBtn = FindViewById<Button>(Resource.Id.publishBtn);
            publishBtn.Click += PublishBtn_Click;
        }

        private void PublishBtn_Click(object sender, EventArgs e)
        {
            PostData();
            Toast.MakeText(this, "PUBLISHED", ToastLength.Short).Show();
            GoToMain();
        }

        private void GoToMain()
        {
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        private void PostData()
        {
            var payload = GetModel().ToJson();
            var client = new RestClient("http://k-app.azurewebsites.net/api/publish/persist");
            var request = new RestRequest(Method.POST);
            request.AddHeader("token", this.GetToken());
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", payload, ParameterType.RequestBody);
            var response = client.Execute(request);
        }

        private ArticlePublish GetModel()
        {
            var result = new ArticlePublish()
            {
                Url = FindViewById<EditText>(Resource.Id.urlTxt).Text,
                Wordpress = FindViewById<CheckBox>(Resource.Id.wpChk).Checked,
                Facebook = FindViewById<CheckBox>(Resource.Id.fbCheck).Checked,
                Twitter = FindViewById<CheckBox>(Resource.Id.twitterChk).Checked,
                LinkedIn = FindViewById<CheckBox>(Resource.Id.linkedInChk).Checked
            };
            return result;
        } 

    }
}