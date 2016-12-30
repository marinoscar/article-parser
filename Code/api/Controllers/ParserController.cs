using api.core.Models;
using api.core.Provider;
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
        public HttpResponseMessage Get(string url)
        {
            var parser = new Parser(Map.I.Container);
            return ErrorHandler.ExecuteCreate<ParserResult>(Request, () => {
                return parser.ParseFromUrl(url);
            });
        }

        [SwaggerOperation("GetArticles")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [ActionName("GetArticles")]
        [HttpGet]
        public HttpResponseMessage GetArticles()
        {
            var parser = new Parser(Map.I.Container);
            return ErrorHandler.ExecuteCreate<IEnumerable<ContentDto>>(Request, () => {
                return parser.GetArticles();
            });
        }

        [SwaggerOperation("Persist")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public HttpResponseMessage Post(ParserResult value)
        {
            var parser = new Parser(Map.I.Container);
            return ErrorHandler.ExecuteCreate<string>(Request, () => {
                parser.Persist(value);
                return value.Id;
            });
        }

        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [HttpPost()]
        [ActionName("Create")]
        public HttpResponseMessage Create(ParseOptions value)
        {
            var parser = new Parser(Map.I.Container);
            return ErrorHandler.ExecuteCreate<ParserResult>(Request, () => {
                var result = parser.Parse(value);
                parser.Persist(result);
                return result;
            });
        }
    }
}
