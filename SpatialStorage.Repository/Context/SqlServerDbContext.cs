using Microsoft.EntityFrameworkCore;

namespace SpatialStorage.Repository.Context
{
    /// <summary>
    /// Context for a SQL database.
    /// </summary>
    /// <seealso cref="DbContext" />
    public class SqlServerDbContext : DbContext
    {
        private readonly ConnectionProvider _connectionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDbContext"/> class.
        /// </summary>
        /// <param name="connectionProvider">The connection provider.</param>
        public SqlServerDbContext(ConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder
                    .UseSqlServer(_connectionProvider(),
                        sqlServerOptions => sqlServerOptions
                            .MigrationsHistoryTable("__EFMigrationHistory", "app")
                            .UseNetTopologySuite()
                        #if DEBUG
                            .CommandTimeout(600)
                        #endif
                        );
            }
        }
    }
}