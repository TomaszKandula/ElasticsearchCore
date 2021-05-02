using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ElasticsearchCore
{
    public static class Program
    {
        private static IHostBuilder CreateHostBuilder(string[] AArgs) 
            => Host.CreateDefaultBuilder(AArgs)
                .ConfigureWebHostDefaults(AWebBuilder 
                    => AWebBuilder.UseStartup<Startup>());

        public static void Main(string[] AArgs)
            =>CreateHostBuilder(AArgs).Build().Run();
    }
}
