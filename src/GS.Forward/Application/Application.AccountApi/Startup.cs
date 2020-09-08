using Application.AccountApi.Domain.Config;
using Application.AccountApi.Domain.Req;
using Application.AccountApi.Middleware;
using AutoMapper;
using Common.GrpcLibrary;
using Common.WebApiHelp.Extension;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Application.AccountApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddControllers(options=> {

            //    #region 设置路由前缀
            //    string prefix = Configuration["Prefix"];

            //    if (!string.IsNullOrWhiteSpace(prefix))
            //        options.UseCentralRoutePrefix(new RouteAttribute(prefix));
            //    #endregion

            //});

            services.AddControllers();

            // 添加http2支持
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            #region 添加AccountService

            string accountServiceUri = Configuration["AccountServiceUri"];

            services.AddGrpcClient<AccountLib.AccountLibClient>(options =>
            {
                options.Address = new Uri(accountServiceUri);
            });

            #endregion

            #region autoMapper
            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<RegisterDto, RegisterReq>()
                // 忽略null值
                .ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
                mc.CreateMap<LoginDto, LoginReq>();
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region 配置获取
            services.Configure<AuthAESConfig>(Configuration.GetSection("AuthAES"));
            #endregion

            #region swagger

            services.AddSwaggerGen(s =>
            {
                // 使用全路径，避免相同类名异常
                s.CustomSchemaIds(x => x.FullName);
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Acount web Api文档",
                    Version = "v1"
                });
                //s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey
                //});
                //s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            },
                //            Scheme = "oauth2",
                //            Name = "Bearer",
                //            In = ParameterLocation.Header,

                //        },
                //        new List<string>()
                //    }
                //});

                s.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Application.AccountApi.xml"));

                s.DocumentFilter<ControllerDocumentFilter>();

            });
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //启用swagger
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("v1/swagger.json", "accountApi v1");
            });

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
