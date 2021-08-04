using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PawsitiveScheduling.Data.Breeds;
using PawsitiveScheduling.Utility;
using PawsitivityScheduler.Data.Breeds;
using System;
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

                try
                {
                    var dbUtility = services.GetRequiredService<IDatabaseUtility>();

                    // Add default breed information if it hasn't already been added
                    var testBreed = dbUtility.GetBreed(BreedNames.Affenpinscher);
                    if (testBreed == null)
                    {
                        await new BreedInitializer(dbUtility).Initialize().ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB");
                }
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
