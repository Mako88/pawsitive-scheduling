using PawsitivityScheduler.Data;
using PawsitivityScheduler.Data.Breeds;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Utility
{
    /// <summary>
    /// Utility for interacting with the database
    /// </summary>
    public interface IDatabaseUtility
    {
        /// <summary>
        /// Get a breed by name
        /// </summary>
        public Task<Breed> GetBreed(BreedNames name);

        /// <summary>
        /// Add a breed to the database
        /// </summary>
        public Task<Breed> AddBreed(Breed breed);

        /// <summary>
        /// Get a dog by id
        /// </summary>
        public Task<Dog> GetDog(string id);

        /// <summary>
        /// Add a dog to the database
        /// </summary>
        public Task<Dog> AddDog(Dog dog);
    }
}