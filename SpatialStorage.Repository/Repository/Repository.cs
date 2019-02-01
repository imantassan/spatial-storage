using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpatialStorage.Repository.Context;
using SpatialStorage.Repository.Entities;

namespace SpatialStorage.Repository.Repository
{
    /// <inheritdoc />
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        private readonly IDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public Repository(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.GetDbSet<TEntity>();
        }

        /// <inheritdoc />
        public Task AddBulkAsync(IEnumerable<TEntity> entities)
        {
            return _dbContext.BulkInsertAsync(entities);
        }

        public Task AddAsync(IEnumerable<TEntity> entities) => _dbSet.AddRangeAsync(entities);

        /// <inheritdoc />
        public Task<int> SaveAsync()
        {
            return _dbContext.SaveAsync();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        /// <inheritdoc />
        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator() => ((IEnumerable<TEntity>) _dbSet).GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _dbSet).GetEnumerator();

        /// <inheritdoc />
        Expression IQueryable.Expression => ((IQueryable) _dbSet).Expression;

        /// <inheritdoc />
        Type IQueryable.ElementType => ((IQueryable) _dbSet).ElementType;

        /// <inheritdoc />
        IQueryProvider IQueryable.Provider => ((IQueryable) _dbSet).Provider;
    }
}