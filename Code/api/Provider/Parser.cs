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
                    PrettyPrint = true
                }
            });
            var content = CleanUp(result.ExtractedContent);
            var keywords = watson.GetKeywords(url);
            var categories = watson.GetTaxonomy(url);
            return new ParserResult()
            {
                Title = result.ExtractedTitle,
                TitleHash = HashManager.GetHashString(result.ExtractedTitle),
                Content = content,
                ContentHash = HashManager.GetHashString(content),
                Url = url,
                ImageUrl = ExtractImage(result.ExtractedContent),
                Author = ExtractAuthor(result.ExtractedContent),
                Keywords = keywords.Items.OrderByDescending(i => i.Relevance).Select(i => i.Text).ToArray().Take(10),
                Categories = categories.Items.Take(5)
            };
        }

        private string ExtractImage(string content)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(content);
            var result = document.QuerySelectorAll("meta[property=\"og:image\"]").FirstOrDefault();
            return result != null ? result.GetAttribute("content") : string.Empty;
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
            result = RemoveStyles(content);
            result = RemoveImages(result);
            return result;
        }

        private string RemoveStyles(string content)
        {
            const string headExp = @"<head>[\s\S]*</head>";
            const string classExp = @"class=(""|')[\s\S]*?(""|')";
            var result = string.Empty;
            result = Regex.Replace(content, headExp, string.Empty);
            result = Regex.Replace(result, classExp, string.Empty);
            return result;
        }

        private string RemoveImages(string content)
        {
            var result = string.Empty;
            const string picExp = @"<picture>[\s\S]*</picture>";
            const string imgExp = @"<img[\s\S]*?>";
            result = Regex.Replace(content, picExp, string.Empty);
            result = Regex.Replace(result, imgExp, string.Empty);
            return result;
        }
    }
}
