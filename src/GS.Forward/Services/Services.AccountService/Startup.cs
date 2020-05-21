using System.Data.Common;
using AccountDomain;
using AccountService.Services;
using AutoMapper;
using Common.GrpcLibrary;
using Common.MySqlProvide;
using Common.MySqlProvide.Generate;
using Common.MySqlProvide.Visitor;
using Dapper;
using GS.AppContext;
using GS.AppContext.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;

namespace AccountService
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

            // ¿ªÆôgrpc
            services.AddGrpc();

            services.AddTransient<DbConnection>(provider => {
                return new MySqlConnection(Configuration.GetConnectionString("MysqlConstr"));
            });

            #region autoMapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<RegisterReq, GSUser>();
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region dbContext

            services.AddTransient<IFrommGnerate, FromGenerateVisitor>();
            services.AddTransient<ISelectGnerate, SelectGenerateVisitor>();
            services.AddTransient<IWhereGnerate, WhereGenerateVisitor>();
            services.AddTransient<IEditGenerate, EditGenerate>();
            services.AddTransient<IQueryContext, MysqlQueryContext>();

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<InfoService>();

                // Ó³Éägrpc·þÎñ
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });

        }
    }
}
