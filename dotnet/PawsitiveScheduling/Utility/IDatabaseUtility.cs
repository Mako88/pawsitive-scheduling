using MongoDB.Bson;
using MongoDB.Driver;
using PawsitiveScheduling.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Utility
{
    /// <summary>
    /// Utility for interacting with the database
    /// </summary>
    public interface IDatabaseUtility
    {
        /// <summary>
        /// Get an entity by ID
        /// </summary>
        public Task<T> GetEntity<T>(ObjectId id) where T : Entity, new();

        /// <summary>
        /// Get entities using a filter expression
        /// </summary>
        public Task<IEnumerable<T>> GetEntities<T>(Expression<Func<T, bool>> filter) where T : Entity, new();

        /// <summary>
        /// Get entities using a filter definition
        /// </summary>
        public Task<IEnumerable<T>> GetEntities<T>(FilterDefinition<T> filter) where T : Entity, new();

        /// <summary>
        /// Add an entity
        /// </summary>
        public Task<T> AddEntity<T>(T entity) where T : Entity, new();

        /// <summary>
        /// Update an entity
        /// </summary>
        public Task<T> UpdateEntity<T>(T entity) where T : Entity, new();

        /// <summary>
        /// Delete an entity
        /// </summary>
        public Task<DeleteResult> DeleteEntity<T>(ObjectId id) where T : Entity, new();

        /// <summary>
        /// Delete an entity, returning it
        /// </summary>
        public Task<T> DeleteAndReturnEntity<T>(ObjectId id) where T : Entity, new();

        /// <summary>
        /// Create an index
        /// </summary>
        Task<string> CreateIndex<T>(Expression<Func<T, object>> definition, string name) where T : Entity, new();
    }
}