using api.Models;
using api.Provider;
using api.Security;
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
            var result = publish.Publish(value);
            return Request.CreateResponse<string>(result.StatusCode, result.Content);
        }
    }
}
