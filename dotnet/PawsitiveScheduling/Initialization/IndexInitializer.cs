using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.DI;
using PawsitivityScheduler.Data.Breeds;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Initialization
{
    /// <summary>
    /// Initialize indexes
    /// </summary>
    [Component]
    public class IndexInitializer : IInitializer
    {
        private readonly IDatabaseUtility dbUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        public IndexInitializer(IDatabaseUtility dbUtility)
        {
            this.dbUtility = dbUtility;
        }

        /// <summary>
        ///  Initialize the indexes
        /// </summary>
        public async Task Initialize()
        {
            await dbUtility.CreateIndex<Breed>(x => x.Name, Constants.BreedNameIndexName);

            await dbUtility.CreateIndex<Appointment>(x => x.ScheduledTime, Constants.AppointmentScheduledTimeIndexName);

            await dbUtility.CreateIndex<Appointment>(x => x.GroomerId, Constants.AppointmentGroomerIdIndexName);
        }
    }
}
