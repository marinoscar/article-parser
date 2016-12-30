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
            var wait = ProgressDialog.Show(this, "Procesing", "Please wait while we complete your request", true, false);
            new Thread(new ThreadStart(delegate
            {
                result = PostData();
                if (!result.Item1)
                {
                    this.ShowDialogOk("Error", result.Item2);
                    wait.Dismiss();
                    return;
                }
                RunOnUiThread(() =>
                {
                    wait.Dismiss();
                    Toast.MakeText(this, result.Item2, ToastLength.Long).Show();
                    GoToMain();
                });
            })).Start();
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
            if (res.StatusCode != HttpStatusCode.Created) return new Tuple<bool, string>(false, "Unable to process the request");
            return new Tuple<bool, string>(true, "The content has been processed");
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