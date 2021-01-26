using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace JiraStatisic.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(builder =>
                {
                    builder
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile("appsettings.local.json", true);
                })
                .ConfigureServices(Startup.ConfigureDelegate);

      
    }
}