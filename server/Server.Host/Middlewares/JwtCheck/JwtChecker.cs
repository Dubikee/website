using Microsoft.AspNetCore.Http;
using Server.Shared.Core.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Host.Middlewares.JwtCheck
{
    public class JwtChecker
    {
        private readonly RequestDelegate _next;

        public JwtChecker(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, IForbiddenJwtStore jwtStore)
        {
            var pair = httpContext.Request.Headers
                .FirstOrDefault(x => x.Key.ToLower() == "authorization");
            var jwt = pair.Value.FirstOrDefault();
            if (jwt != null && jwtStore.IsForbidden(jwt))
            {
                httpContext.Request.Headers.Remove(pair);
                return Task.CompletedTask;
            }

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
}
