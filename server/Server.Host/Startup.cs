using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Server.Service.Extension;
using System;

namespace Server.Host
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAdminService()
                .AddWhutService()
                .AddJwtAuth(opt =>
                {
                    opt.Key = Configuration["AuthOptions:Key"];
                    opt.Audience = Configuration["AuthOptions:Audience"];
                    opt.Issuer = Configuration["AuthOptions:Audience"];
                    opt.UidRegex = Configuration["AuthOptions:UidRegex"];
                    opt.PwdRegex = Configuration["AuthOptions:PwdRegex"];
                    opt.UidClaimType = Configuration["AuthOptions:UidClaimType"];
                    opt.Expires = TimeSpan.FromDays(30);
                })
                .AddAppDbContext(opt =>
                {
                    opt.DbPath = Configuration["DbOptions:DbPath"];
                    opt.UserCollectionName = Configuration["DbOptions:UserCollectionName"];
                    opt.WhutCollectionName = Configuration["DbOptions:WhutCollectionName"];
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory log)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            log.AddNLog();
            env.ConfigureNLog("Nlog.config");
            app.UseAuthentication();
            app.UseMvc(routes => { routes.MapRoute("api", "/api/{controller}/{action}/{uid?}"); });
        }
    }
}
