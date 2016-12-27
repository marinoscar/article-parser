using api.Models;
using api.Security;
using POSSIBLE.WordPress.XmlRpcClient;
using POSSIBLE.WordPress.XmlRpcClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Provider
{
    public class WordpressManager
    {
        private ContentRepository _repository;

        public WordpressManager() : this(new AccountManager(), default(IDataContext))
        {

        }

        public WordpressManager(IAccountManager accountManager, IDataContext dataContext)
        {
            AccountManager = accountManager;
            _repository = new ContentRepository(dataContext);
        }

        public IAccountManager AccountManager { get; private set; }

        public string Post(string postId)
        {
            var item = _repository.GetResult(AccountManager.GetCurrent().Id, postId);
            if (item == null) return string.Empty;
            return Post(item);
        }

        public string Post(IParserResult item)
        {
            var config = WordpressConfig.Load();
            var result = string.Empty;
            using (var wpClient = new WordPressXmlRpcClient(config.Url, config.User, config.Password))
            {
                var post = new Post()
                {
                    post_author = item.Author,
                    post_content = item.FormattedContent,
                    post_date = DateTime.Now,
                    post_title = item.Title,
                    terms = GetTerms(item),
                    custom_fields = GetCustomFields(item)
                };
                result = wpClient.NewPost(post);
            }
            return result;
        }

        private CustomFields[] GetCustomFields(IParserResult item)
        {
            return new CustomFields[] {
                new CustomFields() { key = "title-hash", value = item.TitleHash },
                new CustomFields() { key = "content-hash", value = item.ContentHash },
                new CustomFields() { key = "item-id", value = item.Id },
            };
        }

        private Term[] GetTerms(IParserResult item)
        {
            var terms = new List<Term>();
            foreach (var tag in item.Keywords)
            {
                terms.Add(new Term()
                {
                    taxonomy = "post_tag",
                    name = tag
                });
            }
            foreach (var cat in item.Categories)
            {
                terms.Add(new Term()
                {
                    taxonomy = "category",
                    name = cat
                });
            }
            return terms.ToArray();
        }
    }
}
