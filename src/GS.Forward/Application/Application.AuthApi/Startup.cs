using Application.AuthApi.Configs;
using Application.AuthApi.Middleware;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.AuthApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            // 指定资源验证
            //services.AddScoped<IResourceValidator, ResourceValidator>();

            #region id4

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                //.AddInMemoryApiScopes()
                .AddTestUsers(Config.GetUsers());

            builder.AddDeveloperSigningCredential();

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            IEnumerable<ApiScope> enumerable = app.ApplicationServices.GetService<IEnumerable<ApiScope>>().ToArray();
            IResourceStore resourceStore = app.ApplicationServices.GetService<IResourceStore>();

            Resources resource = resourceStore.FindEnabledResourcesByScopeAsync(new[] { "openid" }).Result;

            ApiScope apiScope = resource.FindApiScope("openId");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

        }
    }
}
