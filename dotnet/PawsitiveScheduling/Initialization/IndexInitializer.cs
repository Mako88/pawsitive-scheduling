using MongoDB.Driver;
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
            var collection = dbUtility.Database.GetCollection<Breed>(Constants.BreedCollectionName);
            var indexKeysDefinition = Builders<Breed>.IndexKeys.Ascending(x => x.Name);
            await collection.Indexes
                .CreateOneAsync(new CreateIndexModel<Breed>(indexKeysDefinition,
                    new CreateIndexOptions { Unique = true, Name = Constants.BreedNameIndexName }))
                .ConfigureAwait(false);
        }
    }
}
