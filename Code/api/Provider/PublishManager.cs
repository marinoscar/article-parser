using api.core.Models;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace api.Provider
{
    public class PublishManager
    {

        private IContainer _container;
        public PublishManager(IContainer container)
        {
            _container = container;
        }

        public PublishResult Publish(PublishOptions options)
        {
            var result = new PublishResult();
            if (options.Parse == null) return result;
            var parseResult = DoParse(options.Parse);
            result.ParseId = parseResult.Id;
            if (options.Article == null) return result;
            var url = DoWordpress(options.Article, parseResult);
            result.WordpressId = parseResult.WordpressId;
            if (options.SocialMedia == null) return result;
            options.SocialMedia.Url = url;
            options.SocialMedia.Text = parseResult.Title;
            result.DidPublishInSocial = DoSocial(options.SocialMedia);
            return result;
        }

        private bool DoSocial(PublishSocialMediaOptions options)
        {
            var socialManager = new SocialMediaManager();
            var res = socialManager.Publish(options);
            return res.StatusCode == HttpStatusCode.OK;
        }

        private string DoWordpress(WordpressOption options, ParserResult parser)
        {
            var wpManager = new WordpressManager(_container);
            var result = string.Empty;
            try { result = wpManager.Post(parser, options); }
            catch (Exception ex) { throw new FriendlyException("Failed to post in wordpress", ex); }
            return result;
        }

        private ParserResult DoParse(ParseOptions options)
        {
            var parse = new Parser(_container);
            var parseResult = parse.Parse(options);
            try { parse.Persist(parseResult); }
            catch (Exception ex) { throw new FriendlyException("Failed to store the result", ex); }
            return parseResult;
        }
    }
}
