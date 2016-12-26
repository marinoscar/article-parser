using api.Models;
using api.Provider;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class ParserController : ApiController
    {
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public ParserResult Get(string url)
        {
            var parser = new Parser();
            return parser.ParseFromUrl(url);
        }
    }
}
