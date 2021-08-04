using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PawsitiveScheduling.Initialization;
using PawsitiveScheduling.Utility;
using System.Threading.Tasks;

namespace PawsitiveScheduling
{
    public class Program
    {
        /// <summary>
        /// Application entry point
        /// </summary>
        public static async Task Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();

            using (var serviceScope = webHost.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                await new Initializer(services.GetRequiredService<IDatabaseUtility>(),
                    services.GetRequiredService<ILogger<Initializer>>()).Initialize().ConfigureAwait(false);
            }

            await webHost.RunAsync().ConfigureAwait(false);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
