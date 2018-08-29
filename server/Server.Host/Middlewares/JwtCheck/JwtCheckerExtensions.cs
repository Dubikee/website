﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Server.Shared.Core.Services;
using Server.Shared.Options;

namespace Server.Host.Middlewares.JwtCheck
{
    public static class JwtCheckerExtensions
    {
        public static IApplicationBuilder UseJwtChecker(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtChecker>();
        }

        public static IServiceCollection AddJwtChecker(this IServiceCollection services)
        {
            return services.AddScoped<IForbiddenJwtStore, IForbiddenJwtStore>();
        }
    }
}