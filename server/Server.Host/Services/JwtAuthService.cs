using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Server.Service.JwtAuth;
using Server.Shared.Core;
using Server.Shared.Models;
using Server.Shared.Options;

namespace Server.Host.Services
{
    public static class JwtAuthService
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services, Action<JwtOptions> optionAction = null)
        {
            var op = new JwtOptions();
            optionAction?.Invoke(op);
            services.TryAddSingleton(op);
            services.TryAddScoped<IAccountManager<IUser>, AccountManager>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(op.Key)),

                    ValidateIssuer = true,
                    ValidIssuer = op.Issuer,

                    ValidateAudience = true,
                    ValidAudience = op.Audience
                };
            });
            services.AddAuthorization(o =>
            {
                o.AddPolicy("AdminOnly", p => p.RequireRole(RoleTypes.Admin, RoleTypes.Master));
                o.AddPolicy("MasterOnly", p => p.RequireRole(RoleTypes.Master));
            });
            return services;
        }
    }
}
