using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace api
{
    public static class ErrorHandler
    {
        public static HttpResponseMessage Execute<T>(HttpRequestMessage request, HttpStatusCode status, Func<T> action)
        {
            var response = default(HttpResponseMessage);
            var result = default(T);
            try
            {
                result = action();
                response = request.CreateResponse<T>(status, result);
            }
            catch (Exception ex)
            {
                response = request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Unable to process request\n\n{0}", ex.Message));
            }
            return response;
        }

        public static HttpResponseMessage ExecuteCreate<T>(HttpRequestMessage request, Func<T> action)
        {
            return Execute<T>(request, HttpStatusCode.Created, action);
        }

        public static HttpResponseMessage ExecuteOk<T>(HttpRequestMessage request, Func<T> action)
        {
            return Execute<T>(request, HttpStatusCode.OK, action);
        }
    }
}