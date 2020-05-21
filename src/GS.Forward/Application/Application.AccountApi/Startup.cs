using Application.AccountApi.Domain.Req;
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
using System;
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
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<RegisterDto, RegisterReq>();
                mc.CreateMap<LoginDto, LoginReq>();
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
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

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
