using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SpatialStorage.Repository.Context;

namespace SpatialStorage.Repository.Repository
{
    /// <inheritdoc />
    public class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : class, new()
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ReadOnlyRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        /// <inheritdoc />
        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator() => _dbContext.GetDbSet<TEntity>().AsNoTracking().GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _dbContext.GetDbSet<TEntity>().AsNoTracking()).GetEnumerator();

        /// <inheritdoc />
        Expression IQueryable.Expression => _dbContext.GetDbSet<TEntity>().AsNoTracking().Expression;

        /// <inheritdoc />
        Type IQueryable.ElementType => _dbContext.GetDbSet<TEntity>().AsNoTracking().ElementType;

        /// <inheritdoc />
        IQueryProvider IQueryable.Provider => _dbContext.GetDbSet<TEntity>().AsNoTracking().Provider;
    }
}