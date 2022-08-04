using MongoDB.Driver;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Entities.Users;
using PawsitivityScheduler.Entities.Dogs.Breeds;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Utility.Database
{
    /// <summary>
    /// Utility for interacting with the database
    /// </summary>
    public interface IDatabaseUtility
    {
        /// <summary>
        /// Get an entity by ID
        /// </summary>
        Task<T?> GetEntity<T>(string? id) where T : Entity;

        /// <summary>
        /// Get entities, optionally filtering and sorting them
        /// </summary>
        Task<List<T>> GetEntities<T>(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? sortBy = null) where T : Entity;

        /// <summary>
        /// Add an entity
        /// </summary>
        Task<T> AddEntity<T>(T entity) where T : Entity;

        /// <summary>
        /// Update an entity
        /// </summary>
        Task<T> UpdateEntity<T>(T entity) where T : Entity;

        /// <summary>
        /// Delete an entity
        /// </summary>
        Task<DeleteResult> DeleteEntity<T>(string id) where T : Entity;

        /// <summary>
        /// Delete an entity, returning it
        /// </summary>
        Task<T?> DeleteAndReturnEntity<T>(string id) where T : Entity;

        /// <summary>
        /// Gets the tracker
        /// </summary>
        Task<Tracker> GetTracker();

        /// <summary>
        /// Create an index
        /// </summary>
        Task<string> CreateIndex<T>(Expression<Func<T, object?>> definition, string name) where T : Entity;

        /// <summary>
        /// Get all appointments that intersect the given start and end dates
        /// </summary>
        Task<IEnumerable<Appointment>> GetAppointments(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get a user by email
        /// </summary>
        Task<User?> GetUserByEmail(string? email);

        /// <summary>
        /// Get a breed by name
        /// </summary>
        Task<Breed?> GetBreedByName(BreedName name);
    }
}