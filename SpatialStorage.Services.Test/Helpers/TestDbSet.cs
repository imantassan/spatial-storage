using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SpatialStorage.Repository.Repository;

namespace SpatialStorage.Services.Test.Helpers
{
    public class TestRepository<TEntity> : IReadOnlyRepository<TEntity>, IAsyncEnumerable<TEntity>
        where TEntity : class, new()
    {
        private readonly IQueryable<TEntity> _entities;

        public TestRepository(IEnumerable<TEntity> source)
        {
            _entities = source.AsQueryable();
        }

        Type IQueryable.ElementType => _entities.ElementType;

        Expression IQueryable.Expression => _entities.Expression;

        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<TEntity>(_entities.Provider);

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator() => _entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _entities.GetEnumerator();

        public IAsyncEnumerator<TEntity> GetEnumerator() => new TestAsyncEnumerator<TEntity>(_entities.GetEnumerator());

        public void Dispose() { }
    }
}