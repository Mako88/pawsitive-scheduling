using PawsitivityScheduler.Data;
using PawsitivityScheduler.Data.Breeds;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Data.Breeds
{
    /// <summary>
    /// Class to add default breed information to the database
    /// </summary>
    public static class BreedInitializer
    {
        /// <summary>
        /// Add default breed information to the database
        /// </summary>
        public static async Task Initialize(SchedulingContext context)
        {
            context.Breeds.Add(
                new Breed { Name = BreedNames.Affenpinscher, Group = Groups.ToyBreeds, Size = Sizes.Medium, BathMinutes = 15, GroomMinutes = 15 }
            );

            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
