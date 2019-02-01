using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SpatialStorage.Repository.Context
{
    /// <inheritdoc />
    /// <summary>
    /// Contains all the methods needed for interaction with a database.
    /// </summary>
    /// <seealso cref="T:System.IDisposable" />
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Submits pending operations to the database asynchronously.
        /// </summary>
        /// <returns>
        /// Number of rows affected.
        /// </returns>
        Task<int> SaveAsync();

        /// <summary>
        /// Gets the database set for entities of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The database set.</returns>
        DbSet<TEntity> GetDbSet<TEntity>()
            where TEntity : class;

        /// <summary>
        /// Migrates the database to the latest version. Creates the database if it doesn't exist.
        /// </summary>
        void Migrate();

        /// <summary>
        /// Bulk inserts the entities suppressing any change tracking.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities to insert.</typeparam>
        /// <param name="entities">The entities to insert.</param>
        Task BulkInsertAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class, new();

        /// <summary>
        /// Performs the specified SQL command.
        /// </summary>
        /// <param name="sqlCommand">The SQL command to perform.</param>
        void Sql(string sqlCommand);
    }
}