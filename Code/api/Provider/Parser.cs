using api.Models;
using NReadability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Provider
{
    public class Parser
    {
        public ParserResult ParseFromUrl(string url)
        {
            var transcoder = new NReadabilityWebTranscoder();
            var result = transcoder.Transcode(new WebTranscodingInput(url) {
            });
            return new ParserResult()
            {
                Title = result.ExtractedTitle,
                Content = result.ExtractedContent,
                Url = url
            };
        }
    }
}
