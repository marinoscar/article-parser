using api.core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace api.Security
{
    public class TokenAuthentication : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple { get { return false; } }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            await Task.Run(() => {
                var token = GetToken(context.Request);
                if(string.IsNullOrWhiteSpace(token))
                    context.ErrorResult = new TokenAuthenticationFailureResult("Token not provided", context.Request);
                if (!TokenManager.IsValid(token))
                    context.ErrorResult = new TokenAuthenticationFailureResult(string.Format("{0} is not a valid token", token), context.Request);
            });
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.Run(() => {

            });
        }

        private string GetToken(HttpRequestMessage request)
        {
            if (!request.Headers.Contains("token")) return null;
            return request.Headers.GetValues("token").FirstOrDefault();
        }
    }
}
