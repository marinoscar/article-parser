using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace app.android
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/logo72x72")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            SetupEvents();
        }

        private void SetupEvents()
        {
            var saveArticleBtn = FindViewById<Button>(Resource.Id.saveContent);
            saveArticleBtn.Click += SaveArticleBtn_Click;
            var articleListBtn = FindViewById<Button>(Resource.Id.viewArticles);
            articleListBtn.Click += ArticleListBtn_Click;
        }

        private void ArticleListBtn_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ArticleList));
            StartActivity(intent);
        }

        private void SaveArticleBtn_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(SaveArticle));
            StartActivity(intent);
        }
    }
}

