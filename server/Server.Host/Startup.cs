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
            services.AddJwtAuth(opt =>
            {
                opt.Key = Configuration["JwtOptions:Key"];
                opt.Audience = Configuration["JwtOptions:Audience"];
                opt.Issuer = Configuration["JwtOptions:Audience"];
                opt.Expires = TimeSpan.FromDays(30);
            }).AddUserDbContext(opt =>
            {
                opt.UserDbPath = Configuration["DbOptions:UserDbPath"];
                opt.CollectionName = Configuration["DbOptions:CollectionName"];
            }).AddAdminService();
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
