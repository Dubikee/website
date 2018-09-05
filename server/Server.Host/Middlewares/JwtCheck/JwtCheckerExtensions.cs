using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Server.Service.Auth;
using Server.Shared.Core.Services;

namespace Server.Host.Middlewares.JwtCheck
{
    public static class JwtCheckerExtensions
    {

        public static IApplicationBuilder UseJwtChecker(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtChecker>();
        }

        public static IServiceCollection AddJwtChecker(this IServiceCollection services, bool eable = false)
        {
            return services.AddScoped<IForbiddenJwtStore, ForbiddenJwtStore>();
        }
    }
}