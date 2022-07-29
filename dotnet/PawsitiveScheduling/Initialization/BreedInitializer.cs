using MongoDB.Driver;
using PawsitiveScheduling.Utility.Database;
using PawsitiveScheduling.Utility.DI;
using PawsitivityScheduler.Entities.Dogs.Breeds;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Initialization
{
    /// <summary>
    /// Class to add default breed information to the database
    /// </summary>
    [Component]
    public class BreedInitializer : IInitializer
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
            // Add default breed information if it hasn't already been added
            var existingBreeds = await dbUtility.GetEntities<Breed>();
            if (existingBreeds.Count() != Enum.GetNames(typeof(BreedName)).Length)
            {
                await dbUtility.AddEntity(
                    new Breed { Name = BreedName.Affenpinscher, Group = Group.ToyBreeds, Size = Size.Medium, BathMinutes = 15, GroomMinutes = 15 }
                );
            }
        }
    }
}
