using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Entities.Users;
using PawsitiveScheduling.Utility.Database;
using PawsitiveScheduling.Utility.DI;
using PawsitivityScheduler.Entities.Dogs.Breeds;
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

            await dbUtility.CreateIndex<User>(x => x.Email, Constants.UserEmailIndexName);
        }
    }
}
