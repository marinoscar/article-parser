using api.Models;
using api.Security;
using POSSIBLE.WordPress.XmlRpcClient;
using POSSIBLE.WordPress.XmlRpcClient.Models;
using StructureMap;
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

        public WordpressManager(IContainer container)
        {
            _repository = new ContentRepository(container);
        }

        public string Post(WordpressOption options)
        {
            var item = _repository.GetResult(options.ArticleId);
            if (item == null) return string.Empty;
            return Post(item, options);
        }

        public string Post(ParserResult item, WordpressOption options)
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
                    custom_fields = GetCustomFields(item),
                    post_status = options.PostStatus,
                    post_excerpt = options.PostExcerpt,
                };
                result = wpClient.NewPost(post);
            }
            return string.Format("{0}?p={1}", config.Url, result);
        }

        private CustomFields[] GetCustomFields(ParserResult item)
        {
            return new CustomFields[] {
                new CustomFields() { key = "title-hash", value = item.TitleHash },
                new CustomFields() { key = "content-hash", value = item.ContentHash },
                new CustomFields() { key = "item-id", value = item.Id },
                new CustomFields() { key = "posted-by-marin-api", value = "true" },
            };
        }

        private Term[] GetTerms(ParserResult item)
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
