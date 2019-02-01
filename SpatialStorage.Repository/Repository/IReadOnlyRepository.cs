using System;
using System.Linq;
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
    public interface IReadOnlyRepository<out TEntity> : IDisposable, IQueryable<TEntity>
        where TEntity : class, new()
    {
    }
}