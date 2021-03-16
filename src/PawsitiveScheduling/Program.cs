using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PawsitiveScheduling.Data.Breeds;
using PawsitivityScheduler.Data;
using PawsitivityScheduler.Data.Breeds;
using System.Linq;
using System.Threading.Tasks;

namespace PawsitiveScheduling
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();

            using (var scope = webHost.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SchedulingContext>();

                await context.Database.MigrateAsync().ConfigureAwait(false);

                // Add default breed information if it hasn't already been added
                var testBreed = context.Breeds.FirstOrDefault(x => x.Name == BreedNames.Affenpinscher);
                if (testBreed == null)
                {
                    await BreedInitializer.Initialize(context).ConfigureAwait(false);
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
