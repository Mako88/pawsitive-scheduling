using Microsoft.Extensions.Logging;
using PawsitiveScheduling.Utility;
using PawsitivityScheduler.Data.Breeds;
using System;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Initialization
{
    /// <summary>
    /// Class to initialize the system
    /// </summary>
    public class Initializer
    {
        private readonly IDatabaseUtility dbUtility;
        private readonly ILogger<Initializer> log;

        /// <summary>
        /// Constructor
        /// </summary>
        public Initializer(IDatabaseUtility dbUtility, ILogger<Initializer> log)
        {
            this.dbUtility = dbUtility;
            this.log = log;
        }

        /// <summary>
        /// Initialize the system
        /// </summary>
        public async Task Initialize()
        {
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

            return;
        }
    }
}
