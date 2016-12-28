using api.Models;
using api.Provider;
using api.Security;
using Newtonsoft.Json;
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
    public class ParserController : ApiController
    {
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public ParserResult Get(string url)
        {
            var parser = new Parser(Map.I.Container);
            return parser.ParseFromUrl(url);
        }

        [SwaggerOperation("Persist")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public HttpResponseMessage Post(ParserResult value)
        {
            var parser = new Parser(Map.I.Container);
            parser.Persist(value);
            return Request.CreateResponse<string>(HttpStatusCode.Created, value.Id);
        }

        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [HttpPost()]
        [ActionName("Create")]
        public HttpResponseMessage Create(ParseOptions value)
        {
            var parser = new Parser(Map.I.Container);
            var result = parser.Parse(value);
            parser.Persist(result);
            return Request.CreateResponse<string>(HttpStatusCode.Created, JsonConvert.SerializeObject(result));
        }
    }
}
