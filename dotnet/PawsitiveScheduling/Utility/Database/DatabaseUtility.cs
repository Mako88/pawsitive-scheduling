using MongoDB.Driver;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Utility.Attributes;
using PawsitiveScheduling.Utility.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Utility.Database
{
    /// <summary>
    /// Utility for interacting with the database
    /// </summary>
    [Component(Singleton = true)]
    public partial class DatabaseUtility : IDatabaseUtility
    {
        private readonly IMongoDatabase database;

        private static readonly Dictionary<Type, string> CollectionNames = new();

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseUtility()
        {
            var client = new MongoClient(EnvironmentVariables.GetDBConnectionString());
            database = client.GetDatabase(Constants.DatabaseName);
        }

        /// <summary>
        /// Get an entity by ID
        /// </summary>
        public async Task<T> GetEntity<T>(string id) where T : Entity =>
            await GetCollection<T>().Find(x => x.Id == id).SingleOrDefaultAsync().ConfigureAwait(false);

        /// <summary>
        /// Get entities, optionally filtering and sorting them
        /// </summary>
        public async Task<List<T>> GetEntities<T>(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> sortBy = null) where T : Entity
        {
            if (filter == null)
            {
                filter = _ => true;
            }

            var finder = GetCollection<T>().Find(filter);

            if (sortBy != null)
            {
                finder = finder.SortBy(sortBy);
            }

            return await finder.ToListAsync();
        }

        /// <summary>
        /// Add an entity
        /// </summary>
        public async Task<T> AddEntity<T>(T entity) where T : Entity
        {
            var collection = GetCollection<T>();

            await collection.InsertOneAsync(entity).ConfigureAwait(false);

            return entity;
        }

        /// <summary>
        /// Update an entity
        /// </summary>
        public async Task<T> UpdateEntity<T>(T entity) where T : Entity
        {
            var collection = GetCollection<T>();

            await collection.ReplaceOneAsync(x => x.Id == entity.Id, entity).ConfigureAwait(false);

            return entity;
        }

        /// <summary>
        /// Delete an entity
        /// </summary>
        public async Task<DeleteResult> DeleteEntity<T>(string id) where T : Entity =>
            await GetCollection<T>().DeleteOneAsync(x => x.Id == id).ConfigureAwait(false);

        /// <summary>
        /// Delete an entity, returning it
        /// </summary>
        public async Task<T> DeleteAndReturnEntity<T>(string id) where T : Entity
        {
            var entity = await GetEntity<T>(id).ConfigureAwait(false);

            await DeleteEntity<T>(id).ConfigureAwait(false);

            return entity;
        }

        /// <summary>
        /// Gets the tracker
        /// </summary>
        public async Task<Tracker> GetTracker() => (await GetEntities<Tracker>()).SingleOrDefault();

        /// <summary>
        /// Perform initialization
        /// </summary>
        public static void Initialize()
        {
            var entityTypes = Assembly.GetAssembly(typeof(Entity)).GetTypes().Where(x => x.IsSubclassOf(typeof(Entity)));

            foreach (var entityType in entityTypes)
            {
                var attribute = (BsonCollectionNameAttribute) Attribute.GetCustomAttribute(entityType, typeof(BsonCollectionNameAttribute));

                if (attribute == null)
                {
                    throw new Exception($"The {entityType.Name} entity does not have the required BsonCollectionName attribute");
                }

                CollectionNames.Add(entityType, attribute.CollectionName);
            }
        }

        /// <summary>
        /// Create an index
        /// </summary>
        public async Task<string> CreateIndex<T>(Expression<Func<T, object>> definition, string name) where T : Entity
        {
            var collection = GetCollection<T>();
            var index = Builders<T>.IndexKeys.Ascending(definition);

            return await collection.Indexes
                .CreateOneAsync(new CreateIndexModel<T>(index, new CreateIndexOptions { Unique = true, Name = name }))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get the collection for the given type
        /// </summary>
        protected IMongoCollection<T> GetCollection<T>() where T : Entity =>
            database.GetCollection<T>(CollectionNames[typeof(T)]);
    }
}
