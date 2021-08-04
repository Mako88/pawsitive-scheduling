using PawsitiveScheduling.Utility;
using PawsitivityScheduler.Data.Breeds;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Data.Breeds
{
    /// <summary>
    /// Class to add default breed information to the database
    /// </summary>
    public class BreedInitializer
    {
        private readonly IDatabaseUtility dbUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        public BreedInitializer(IDatabaseUtility dbUtility)
        {
            this.dbUtility = dbUtility;
        }

        /// <summary>
        /// Add default breed information to the database
        /// </summary>
        public async Task Initialize()
        {
            await dbUtility.AddBreed(
                new Breed { Name = BreedNames.Affenpinscher, Group = Groups.ToyBreeds, Size = Sizes.Medium, BathMinutes = 15, GroomMinutes = 15 }
            ).ConfigureAwait(false);
        }
    }
}
