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
using app.core;

namespace app.android
{
    [Activity(Label = "@string/ViewArticleTitle")]
    public class ArticleList : Activity
    {
        private List<ContentDto> _items;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ArticleList);
            var manager = new ContentManager(this);
            var list = FindViewById<ListView>(Resource.Id.artcileListView);
            this.ExecuteLongTask(() => {
                _items = manager.GetArticles().ToList();
            }, () => {
                if (_items.Count <= 0)
                    _items.Add(new ContentDto() { Title = "NO ARTICLES" });
                var adapter = new ArticleAdapter(this, _items);
                list.Adapter = adapter;
                list.ItemClick += List_ItemClick;
            },
                "Processing", "Please wait while we get the data for you!");
        }
        private void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = _items[e.Position];
            if (item == null) return;
            var uri = Android.Net.Uri.Parse(string.Format("http://blog.marin.cr/?p={0}", item.WordpressId));
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);

        }
    }

    public class ArticleAdapter : BaseAdapter<ContentDto>
    {

        public ArticleAdapter(Activity context, IEnumerable<ContentDto> items)
        {
            Context = context;
            Items = items.ToList();
        }

        public Activity Context { get; private set; }
        public List<ContentDto> Items { get; private set; }

        public override ContentDto this[int position] { get { return Items[position]; } }

        public override int Count { get { return Items.Count; } }

        public override long GetItemId(int position) { return position; }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = Items[position];
            var view = convertView;
            if(view == null)
                view = Context.LayoutInflater.Inflate(Resource.Layout.ArticleItem, null);
            view.FindViewById<TextView>(Resource.Id.articleItemTitle).Text = item.Title;
            view.FindViewById<TextView>(Resource.Id.articleItemDesc).Text = item.Excerpt;
            return view;
        }
    }
}