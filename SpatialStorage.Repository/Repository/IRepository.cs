using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SpatialStorage.Repository.Entities;

namespace SpatialStorage.Repository.Repository
{
    /// <inheritdoc cref="IQueryable{T}" />
    /// <summary>
    /// Contains all the methods needed for a repository controlling a specific type of entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities controlled by the repository.</typeparam>
    /// <seealso cref="T:System.IDisposable" />
    /// <seealso cref="T:System.Linq.IQueryable`1" />
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// Adds the collection of entities to database context asynchronously bypassing change tracking.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        Task AddBulkAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Adds the collection of entities to database context.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        Task AddAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Submits all the pending operations to underlying database context.
        /// </summary>
        /// <returns>The number of entities affected.</returns>
        Task<int> SaveAsync();
    }
}