﻿using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using api.core.Models;
using api.core.Security;
using NReadability;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace api.core.Provider
{
    public class Parser
    {

        private IContainer _container;

        public Parser(IContainer container)
        {
            _container = container;
            AccountManager = _container.GetInstance<IAccountManager>();
        }

        public IAccountManager AccountManager { get; private set; }

        public ParserResult ParseFromUrl(string url)
        {
            return Parse(new ParseOptions() { Url = url });
        }

        public ParserResult Parse(ParseOptions options)
        {
            var watson = new WatsonNavigator();
            var transcoder = new NReadabilityWebTranscoder();
            var result = transcoder.Transcode(new WebTranscodingInput(options.Url)
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
            var keywords = watson.GetKeywords(options.Url);
            var categories = watson.GetTaxonomy(options.Url);
            var imgUrl = ExtractImage(document);
            var rawText = GetRawText(content);
            return new ParserResult()
            {
                UserId = AccountManager.GetCurrent().Id,
                Title = result.ExtractedTitle,
                TitleHash = HashManager.GetHashString(result.ExtractedTitle),
                Content = content,
                ContentHash = HashManager.GetHashString(content),
                FormattedContent = ProvideFormat(content, result.ExtractedTitle, options, imgUrl),
                RawText = rawText,
                Url = options.Url,
                Excerpt = GetExcerpt(rawText, result.ExtractedTitle),
                ImageUrl = imgUrl,
                Author = ExtractAuthor(result.ExtractedContent),
                Keywords = keywords.Items.Where(i => i.Relevance > 0.85).OrderByDescending(i => i.Relevance).Select(i => StringUtils.PretifyWords(i.Text)).ToArray(),
                Categories = categories.Items.Select(i => StringUtils.PretifyWords(i)).Take(10),
                Images = ExtractAllImages(document)
            };
        }


        private string GetExcerpt(string content, string title)
        {
            var words = GetWords(content.Replace(title, "")).Take(50);
            return string.Join(" ", words);

        }

        private string GetRawText(string content)
        {
            var result = Regex.Replace(content, "<.*?>", " ");
            var words = GetWords(result);
            return string.Join(" ", words);
        }

        private IEnumerable<string> GetWords(string content)
        {
            return Regex.Matches(content, @"\w(?<!\d)[\w'-]*").Cast<Match>().Where(i => i.Success).Select(i => i.Value).ToArray();
        }

        private string ProvideFormat(string content, string title, ParseOptions options, string imgUrl)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(content);
            var body = document.GetElementsByTagName("body").FirstOrDefault();
            if (body == null) return content;
            var p = document.CreateElement("p");
            p.InnerHtml = string.Format("<strong>Original Content: <a href='{0}'>{1}</a> </strong>", options.Url, title);
            body.Append(p);
            if (!string.IsNullOrWhiteSpace(imgUrl))
            {
                var img = document.CreateElement("img");
                img.SetAttribute("src", imgUrl);
                if(!string.IsNullOrWhiteSpace(options.ImageClass))
                    img.SetAttribute("class", options.ImageClass);
                else
                    img.SetAttribute("class", "post-image-formatting-class");
                body.Insert(AngleSharp.Dom.AdjacentPosition.AfterBegin, img.OuterHtml);
            }
            RemoveTitle(title, document);
            return document.Body.InnerHtml.Replace("<head></head>", "").Replace("<body>", "").Replace("</body>", "");
        }

        public IEnumerable<ContentDto> GetArticles()
        {
            var contentRepo = new ContentRepository(_container);
            return contentRepo.GetArticles().OrderByDescending(i => i.UtcUpdatedOn).ToList();
        }

        public void Persist(ParserResult value)
        {
            var contentRepo = new ContentRepository(_container);
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

        private void RemoveTitle(string title, IHtmlDocument document)
        {
            var heading = document.All.FirstOrDefault(i => i.TextContent == title);
            if (heading == null) return;
            heading.OuterHtml = "<span data-removed-by-formatter='true'></span>";
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
