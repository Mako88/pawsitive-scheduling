using MongoDB.Driver;
using PawsitivityScheduler.Data;
using PawsitivityScheduler.Data.Breeds;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Utility
{
    /// <summary>
    /// Utility for interacting with the database
    /// </summary>
    public class DatabaseUtility : IDatabaseUtility
    {
        private const string DatabaseName = "pawsitivity";
        private const string BreedCollectionName = "breeds";
        private const string DogCollectionName = "dogs";

        private readonly IMongoDatabase database;

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseUtility()
        {
            var client = new MongoClient(EnvironmentVariables.GetDBConnectionString());
            database = client.GetDatabase(DatabaseName);
        }

        /// <summary>
        /// Get a breed by name
        /// </summary>
        public async Task<Breed> GetBreed(BreedNames name)
        {
            var collection = database.GetCollection<Breed>(BreedCollectionName);

            return await collection.Find(x => x.Id == name).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Add a breed to the database
        /// </summary>
        public async Task<Breed> AddBreed(Breed breed)
        {
            var collection = database.GetCollection<Breed>(BreedCollectionName);

            await collection.InsertOneAsync(breed).ConfigureAwait(false);

            return breed;
        }

        /// <summary>
        /// Get a dog by id
        /// </summary>
        public async Task<Dog> GetDog(string id)
        {
            var collection = database.GetCollection<Dog>(DogCollectionName);

            return await collection.Find(x => x.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Add a dog to the database
        /// </summary>
        public async Task<Dog> AddDog(Dog dog)
        {
            var collection = database.GetCollection<Dog>(DogCollectionName);

            await collection.InsertOneAsync(dog).ConfigureAwait(false);

            return dog;
        }
    }
}
