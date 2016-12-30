using api.core.Models;
using api.core.Provider;
using api.core.Security;
using NUnit.Framework;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.tests
{
 
    [TestFixture]   
    public class Integration
    {

        private IContainer _container;

        [Test]
        public void DoFullIntegration()
        {
            var options = new PublishOptions()
            {
                Parse = new ParseOptions() { Url = "https://techcrunch.com/2016/12/28/the-price-of-bitcoin-is-creeping-back-toward-its-3-year-high-of-1000/" },
                Article = new WordpressOption() { PostStatus = "draft" },
                SocialMedia = new PublishSocialMediaOptions()
                {
                    DoFacebook = true,
                    DoLinkedIn = true,
                    DoTwitter = true
                }
            };
            var publishManager = new PublishManager(_container);
            var result = publishManager.Publish(options);
            Assert.IsNotNull(result);
        }

        [Test]
        public void DoIntegrationWithoutSocialMedia()
        {
            var options = new PublishOptions()
            {
                Parse = new ParseOptions() { Url = "https://techcrunch.com/2016/12/28/the-price-of-bitcoin-is-creeping-back-toward-its-3-year-high-of-1000/" },
                Article = new WordpressOption() { PostStatus = "publish" },
            };
            var publishManager = new PublishManager(_container);
            var result = publishManager.Publish(options);
            Assert.IsNotNull(result);
        }

        [SetUp]
        public void Initialize()
        {
            _container = new Container(i =>
            {
                i.For<IDataContext>().Use<MongoDataContext>().Ctor<string>().Is("kapp");
                i.For<IAccountManager>().Use<AccountManager>();
            });
        }


    }
}
