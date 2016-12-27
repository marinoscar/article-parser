using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using api.Models;
using api.Security;
using NReadability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace api.Provider
{
    public class Parser
    {
        public Parser() : this(new AccountManager())
        {

        }

        public Parser(IAccountManager accountManager)
        {
            AccountManager = accountManager;
        }

        public IAccountManager AccountManager { get; private set; }

        public ParserResult ParseFromUrl(string url)
        {
            var watson = new WatsonNavigator();
            var transcoder = new NReadabilityWebTranscoder();
            var result = transcoder.Transcode(new WebTranscodingInput(url)
            {
                DomSerializationParams = new DomSerializationParams()
                {
                    DontIncludeContentTypeMetaElement = true,
                    DontIncludeDocTypeMetaElement = true,
                    DontIncludeGeneratorMetaElement = true,
                    DontIncludeMobileSpecificMetaElements = true,
                    PrettyPrint = false
                }
            });
            var parser = new HtmlParser();
            var document = parser.Parse(result.ExtractedContent);
            var content = CleanUp(result.ExtractedContent);
            var keywords = watson.GetKeywords(url);
            var categories = watson.GetTaxonomy(url);
            var imgUrl = ExtractImage(document);
            return new ParserResult()
            {
                UserId = AccountManager.GetCurrent().Id,
                Title = result.ExtractedTitle,
                TitleHash = HashManager.GetHashString(result.ExtractedTitle),
                Content = content,
                ContentHash = HashManager.GetHashString(content),
                FormattedContent = AddImageAndSource(content, result.ExtractedTitle, url, imgUrl),
                Url = url,
                ImageUrl = imgUrl,
                Author = ExtractAuthor(result.ExtractedContent),
                Keywords = keywords.Items.Where(i => i.Relevance > 0.85).OrderByDescending(i => i.Relevance).Select(i => StringUtils.PretifyWords(i.Text)).ToArray(),
                Categories = categories.Items.Select(i => StringUtils.PretifyWords(i)).Take(10),
                Images = ExtractAllImages(document)
            };
        }

        private string AddImageAndSource(string content, string title, string url, string imgUrl)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(content);
            var body = document.GetElementsByTagName("body").FirstOrDefault();
            if (body == null) return content;
            var p = document.CreateElement("p");
            p.InnerHtml = string.Format("<strong>Original Content: <a href='{0}'>{1}</a> </strong>", url, title);
            body.Append(p);
            if (!string.IsNullOrWhiteSpace(imgUrl))
            {
                var img = document.CreateElement("img");
                img.SetAttribute("src", imgUrl);
                img.SetAttribute("class", "post-image-formatting-class");
                body.Insert(AngleSharp.Dom.AdjacentPosition.AfterBegin, img.OuterHtml);
            }
            return document.DocumentElement.OuterHtml;
        }

        public void Persist(ParserResult value)
        {
            var contentRepo = new ContentRepository();
            contentRepo.PersistResult(value);
        }

        private string ExtractImage(IHtmlDocument document)
        {
            var result = document.QuerySelectorAll("meta[property=\"og:image\"]").FirstOrDefault();
            return result != null ? result.GetAttribute("content") : string.Empty;
        }

        private IEnumerable<string> ExtractAllImages(IHtmlDocument document)
        {
            return document.QuerySelectorAll("img").Select(i => i.GetAttribute("src")).Where(i => !string.IsNullOrWhiteSpace(i));
            
        }

        private string ExtractAuthor(string content)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(content);
            var result = document.QuerySelectorAll("meta[name=\"author\"]").FirstOrDefault();
            return result != null ? result.GetAttribute("content") : string.Empty;
        }

        private string CleanUp(string content)
        {
            var result = string.Empty;
            result = StringUtils.RemoveStyles(content);
            result = StringUtils.RemoveImages(result);
            return result;
        }
    }
}
