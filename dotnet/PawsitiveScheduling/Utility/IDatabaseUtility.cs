﻿using MongoDB.Driver;
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
        Task<T> GetEntity<T>(string id) where T : Entity;

        /// <summary>
        /// Get entities using a filter expression
        /// </summary>
        Task<IEnumerable<T>> GetEntities<T>(Expression<Func<T, bool>> filter) where T : Entity;

        /// <summary>
        /// Get all entities in a collection
        /// </summary>
        Task<IEnumerable<T>> GetAllEntities<T>() where T : Entity;

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
        Task<T> DeleteAndReturnEntity<T>(string id) where T : Entity;

        /// <summary>
        /// Create an index
        /// </summary>
        Task<string> CreateIndex<T>(Expression<Func<T, object>> definition, string name) where T : Entity;
    }
}