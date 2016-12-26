using api.Models;
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
            var content = RemoveStyles(result.ExtractedContent);
            return new ParserResult()
            {
                Title = result.ExtractedTitle,
                Content = content,
                Url = url
            };
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
    }
}
