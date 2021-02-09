using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Elasticsearch.Net;
using Nest;

namespace ElasticsearchCore
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration AConfiguration)
        {
            Configuration = AConfiguration;
        }

        public void ConfigureServices(IServiceCollection AServices)
        {
            var LPool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var LSettings = new ConnectionSettings(LPool).DefaultIndex("books");
            var LClient = new ElasticClient(LSettings);

            AServices.AddSingleton(LClient);
            AServices.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder ABuilder, IWebHostEnvironment AEnvironment)
        {
            if (AEnvironment.IsDevelopment())
            {
                ABuilder.UseDeveloperExceptionPage();
            }
            else
            {
                ABuilder.UseExceptionHandler("/Main/Error");
                ABuilder.UseHsts();
            }
            ABuilder.UseHttpsRedirection();
            ABuilder.UseStaticFiles();
            ABuilder.UseRouting();
            ABuilder.UseAuthorization();
            ABuilder.UseEndpoints(AEndpoints =>
            {
                AEndpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Main}/{action=Index}/{id?}");
            });
        }
    }
}
