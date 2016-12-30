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

namespace app.android
{
    [Activity(Label = "ArticleList")]
    public class ArticleList : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var manager = new ContentManager(this);
            var list = FindViewById<ListView>(Resource.Id.artcileListView);
            var items = manager.GetArticles();
            var adapter = new ArrayAdapter<string>(this, Resource.Layout.ArticleList, items.Select(i => i.Title).ToArray());
            list.Adapter = adapter;

        }
    }
}