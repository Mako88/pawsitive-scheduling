using MongoDB.Driver;
using PawsitivityScheduler.Entities.Dogs.Breeds;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Utility.Database
{
    /// <summary>
    /// Breed-related database methods
    /// </summary>
    public partial class DatabaseUtility : IDatabaseUtility
    {
        /// <summary>
        /// Get a breed by name
        /// </summary>
        public async Task<Breed> GetBreedByName(BreedName name) =>
            await GetCollection<Breed>().Find(x => x.Name == name).SingleOrDefaultAsync();
    }
}
