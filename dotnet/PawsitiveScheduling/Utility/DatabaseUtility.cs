using MongoDB.Bson;
using MongoDB.Driver;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Utility.DI;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Utility
{
    /// <summary>
    /// Utility for interacting with the database
    /// </summary>
    [Component(Singleton = true)]
    public class DatabaseUtility : IDatabaseUtility
    {
        private readonly IMongoDatabase database;

        /// <summary>
        /// Direct access to the database
        /// </summary>
        public IMongoDatabase Database { get => database; }

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
        public async Task<T> GetEntity<T>(ObjectId id) where T : Entity, new() =>
            await GetCollection<T>().Find(x => x.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);

        /// <summary>
        /// Get entities using a filter expression
        /// </summary>
        public async Task<IEnumerable<T>> GetEntities<T>(Expression<Func<T, bool>> filter) where T : Entity, new() =>
            await GetCollection<T>().Find(filter).ToListAsync();

        /// <summary>
        /// Get entities using a filter definition
        /// </summary>
        public async Task<IEnumerable<T>> GetEntities<T>(FilterDefinition<T> filter) where T : Entity, new() =>
            await GetCollection<T>().Find(filter).ToListAsync();

        /// <summary>
        /// Add an entity
        /// </summary>
        public async Task<T> AddEntity<T>(T entity) where T : Entity, new()
        {
            var collection = GetCollection<T>();

            await collection.InsertOneAsync(entity).ConfigureAwait(false);

            return entity;
        }

        /// <summary>
        /// Add an entity
        /// </summary>
        public async Task<T> UpdateEntity<T>(T entity) where T : Entity, new()
        {
            var collection = GetCollection<T>();

            await collection.InsertOneAsync(entity).ConfigureAwait(false);

            return entity;
        }

        /// <summary>
        /// Delete an entity
        /// </summary>
        public async Task<DeleteResult> DeleteEntity<T>(ObjectId id) where T : Entity, new() =>
            await GetCollection<T>().DeleteOneAsync(x => x.Id == id).ConfigureAwait(false);

        /// <summary>
        /// Delete an entity, returning it
        /// </summary>
        public async Task<T> DeleteAndReturnEntity<T>(ObjectId id) where T : Entity, new()
        {
            var entity = await GetEntity<T>(id).ConfigureAwait(false);

            await DeleteEntity<T>(id).ConfigureAwait(false);

            return entity;
        }

        /// <summary>
        /// Get the collection for the given type
        /// </summary>
        private IMongoCollection<T> GetCollection<T>() where T : Entity, new() =>
            Database.GetCollection<T>(new T().CollectionName);
    }
}
