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
            var result = default(ParserResult);
            var parser = new Parser(Map.I.Container);
            result = parser.ParseFromUrl(url);
            return Request.CreateResponse<ParserResult>(HttpStatusCode.Created, result);
        }

        [SwaggerOperation("Persist")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public HttpResponseMessage Post(ParserResult value)
        {
            var parser = new Parser(Map.I.Container);
            try
            {
                parser.Persist(value);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse((HttpStatusCode.InternalServerError, string.Format("Unable to process request\n\n{0}", ex.Message));
            }
            return Request.CreateResponse<string>(HttpStatusCode.Created, value.Id);
        }

        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [HttpPost()]
        [ActionName("Create")]
        public HttpResponseMessage Create(ParseOptions value)
        {
            var result = default(ParserResult);
            var parser = new Parser(Map.I.Container);
            try
            {
                result = parser.Parse(value);
                parser.Persist(result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse((HttpStatusCode.InternalServerError, string.Format("Unable to process request\n\n{0}", ex.Message));
            }
            return Request.CreateResponse<ParserResult>(HttpStatusCode.Created, result);
        }
    }
}
