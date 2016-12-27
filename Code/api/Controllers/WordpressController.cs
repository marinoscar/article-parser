﻿using api.Models;
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
    public class WordpressController : ApiController
    {
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public HttpResponseMessage Post(string key)
        {
            var wpManager = new WordpressManager();
            var res = wpManager.Post(key);
            return Request.CreateResponse<string>(HttpStatusCode.Created, res);
        }
    }
}
