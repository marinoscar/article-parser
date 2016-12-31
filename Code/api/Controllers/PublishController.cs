using api.core;
using api.core.Models;
using api.core.Provider;
using api.Security;
using Newtonsoft.Json.Linq;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    [TokenAuthentication]
    public class PublishController : ApiController
    {
        [SwaggerOperation("Publish")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public HttpResponseMessage Post(PublishSocialMediaOptions value)
        {
            var publish = new SocialMediaManager();
            return ErrorHandler.ExecuteCreate<string>(Request, () => {
                var result = publish.Publish(value);
                return result.Content;
            });
        }

        [SwaggerOperation("Persist")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [ActionName("Persist")]
        [HttpPost]
        public HttpResponseMessage Persist(PublishOptions value)
        {
            var publish = new PublishManager(Map.I.Container);
            return ErrorHandler.ExecuteCreate<PublishResult>(Request, () => {
                return publish.Publish(value);
            });
        }
    }
}
