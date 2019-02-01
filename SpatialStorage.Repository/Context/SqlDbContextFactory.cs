#if DEBUG
using System;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SpatialStorage.Repository.Context
{
    /// <inheritdoc />
    /// <summary>
    /// Context factory for a SQL database, used only for creating migrations.
    /// </summary>
    /// <seealso cref="T:Microsoft.EntityFrameworkCore.Design.IDesignTimeDbContextFactory`1" />
    [Obsolete("Should not be used within the code, only for migrations.")]
    public class SqlDbContextFactory : IDesignTimeDbContextFactory<SqlServerDbContext>
    {
        /// <inheritdoc />
        public SqlServerDbContext CreateDbContext(string[] args)
        {
            var connectionString = new ConfigurationBuilder()
                .AddJsonFile("migrationsettings.json")
                .Build()
                .GetConnectionString("Migrations");

            // Should only be used for migrations
            return new SqlServerDbContext(() => connectionString);
        }
    }
}
#endif