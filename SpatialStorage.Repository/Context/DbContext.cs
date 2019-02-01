using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SpatialStorage.Repository.Entities;

namespace SpatialStorage.Repository.Context
{
    public delegate string ConnectionProvider();

    public abstract class DbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
    {
        protected static bool IsMigrated { get; private set; }

        public Task<int> SaveAsync()
        {
            return base.SaveChangesAsync();
        }

        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void Migrate()
        {
            if (!IsMigrated)
            {
                IsMigrated = true;
                Database.Migrate();
            }
        }

        public Task BulkInsertAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            return this.BulkInsertAsync(entities.ToList(), new BulkConfig
            {
                BatchSize = 10000,
                TrackingEntities = false
            });
        }

        public void Sql(string sqlCommand)
        {
            base.Database.ExecuteSqlCommand(sqlCommand);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            GeographyLocation.Register(modelBuilder);
            SimpleLocation.Register(modelBuilder);
        }
    }
}