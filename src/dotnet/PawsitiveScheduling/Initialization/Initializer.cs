using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PawsitiveScheduling.Utility;
using PawsitivityScheduler.Data.Breeds;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Initialization
{
    /// <summary>
    /// Class to initialize the system
    /// </summary>
    public class Initializer : IHostedService
    {
        private readonly IServiceScopeFactory scopeFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public Initializer(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        /// <summary>
        /// Initialize the system
        /// </summary>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbUtility = scope.ServiceProvider.GetRequiredService<IDatabaseUtility>();
                var log = scope.ServiceProvider.GetRequiredService<ILogger<Initializer>>();

                try
                {
                    // Add default breed information if it hasn't already been added
                    var testBreed = await dbUtility.GetBreed(BreedNames.Affenpinscher);
                    if (testBreed == null)
                    {
                        await new BreedInitializer(dbUtility).Initialize().ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    log.LogError(ex, "An error occurred seeding the DB");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
