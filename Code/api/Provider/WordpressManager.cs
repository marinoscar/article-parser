using api.Models;
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
        public void Post(ParserResult item)
        {
            var config = WordpressConfig.Load();
            using (var wpClient = new WordPressXmlRpcClient(config.Url, config.User, config.Password))
            {
                var post = new Post()
                {
                    post_author = item.Author,
                    post_content = item.Content,
                    post_date = DateTime.Now,
                    post_title = item.Title,
                    terms = GetTerms(item),
                };
            }
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
