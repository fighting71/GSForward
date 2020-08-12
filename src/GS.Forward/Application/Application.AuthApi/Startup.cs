using Application.AuthApi.Controllers;
using Application.AuthApi.Middleware;
using Application.AuthApi.SourceCode;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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

            // 方便调试
            services.AddTransient<IResourceValidator, ResourceValidator>();
            services.AddTransient<ITokenCreationService, CodeDefaultTokenCreationService>();
            services.AddTransient<ITokenService, CodeDefaultTokenService>();

            #region id4

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddTestUsers(Config.GetUsers());

            builder.AddDeveloperSigningCredential();

            #endregion

            AuthenticationBuilder authenticationBuilder = services.AddAuthentication("Bearer");

            authenticationBuilder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, CodeJwtBearerPostConfigureOptions>());

            authenticationBuilder.AddScheme<JwtBearerOptions, CodeAuthenticationHandler>("Bearer", null, options =>
                 {
                     options.Authority = "http://localhost:5004";
                     options.RequireHttpsMetadata = false;
                     options.Audience = "api1";
                     options.SecurityTokenValidators.Clear();
                     options.SecurityTokenValidators.Add(new CodeJwtSecurityTokenHandler());
                     options.Validate("Bearer");
                 });

            // 显示完整错误信息
            IdentityModelEventSource.ShowPII = true;
            services.AddScoped<CusFilterAttribute>();

            services.AddAuthorization();

            services.AddControllers(options=> {

                Console.WriteLine(options);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //IEnumerable<ApiScope> enumerable = app.ApplicationServices.GetService<IEnumerable<ApiScope>>().ToArray();
            //IResourceStore resourceStore = app.ApplicationServices.GetService<IResourceStore>();

            //Resources resource = resourceStore.FindEnabledResourcesByScopeAsync(new[] { "openid" }).Result;

            //ApiScope apiScope = resource.FindApiScope("openId");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.UseRouting();

            //启用JWT鉴权
            app.UseAuthentication();
            //启用JWT授权
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMiddleware2(typeof(EmptyMiddleware),new object[0]);

        }
    }
}
