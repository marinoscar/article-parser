using api.core.Models;
using api.core.Provider;
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
    public class WordpressController : ApiController
    {
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public HttpResponseMessage Post(WordpressOption value)
        {
            var wpManager = new WordpressManager(Map.I.Container);
            var res = wpManager.Post(value);
            return Request.CreateResponse<string>(HttpStatusCode.Created, res);
        }
    }
}
