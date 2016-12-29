using api.core.Models;
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

        [SwaggerOperation("Persist")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [ActionName("Persist")]
        [HttpPost]
        public HttpResponseMessage Persist(PublishOptions value)
        {
            HttpResponseMessage response;
            var result = default(PublishResult);
            var publish = new PublishManager(Map.I.Container);
            try { result = publish.Publish(value); }
            catch (FriendlyException)
            {
                response = Request.CreateResponse<PublishResult>(HttpStatusCode.Conflict, result);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, "Unable to process the request due to an error");
            }
            return Request.CreateResponse<PublishResult>(HttpStatusCode.Created, result);
        }
    }
}
