using Common.WebApiHelp.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using StackExchange.Redis.ConnectionPool.DependencyInject;
using System.Collections.Generic;
using System.Threading;

namespace Application.QuestionApi
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
            // ����ʵ�ʵ�ҵ������������С�߳���
            ThreadPool.SetMinThreads(200, 200);

            services.AddRedisConnectionPool(new ConfigurationOptions()
            {
                EndPoints = {
                    { "127.0.0.1",6379}
                }
            },200);

            services.AddControllers(options => {

                string prefix = Configuration["Prefix"];

                if (!string.IsNullOrWhiteSpace(prefix))
                {
                    // ����ȫ��·��
                    options.UseCentralRoutePrefix(new RouteAttribute(prefix));
                }

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
