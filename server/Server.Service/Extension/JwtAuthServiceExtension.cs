using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Server.Shared.Core;
using Server.Shared.Models;
using Server.Shared.Options;
using System;
using System.Text;
using Server.Service.Auth;

namespace Server.Service.Extension
{
    public static class JwtAuthServiceExtension
    {
        /// <summary>
        /// 注入身份验证、授权相关
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtAuth(this IServiceCollection services, Action<JwtOptions> optionAction = null)
        {
            var op = new JwtOptions();
            optionAction?.Invoke(op);
            services.TryAddSingleton(op);
            services.TryAddScoped<IAccountManager<User>, AccountManager>();
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
