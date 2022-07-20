using Microsoft.Extensions.Hosting;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.DI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Initialization
{
    /// <summary>
    /// Class to initialize the system
    /// </summary>
    [Component(Singleton = true)]
    public class ServiceInitializer : IHostedService
    {
        private readonly IDatabaseUtility dbUtility;
        private readonly IEnumerable<IInitializer> initializers;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceInitializer(IDatabaseUtility dbUtility, IEnumerable<IInitializer> initializers, ILog log)
        {
            this.dbUtility = dbUtility;
            this.initializers = initializers;
            this.log = log;
        }

        /// <summary>
        /// Initialize the system
        /// </summary>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                foreach (var initializer in initializers)
                {
                    await initializer.Initialize().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                log.Error("An error occurred seeding the DB", ex);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
