using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GeoAPI.Geometries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite;
using SpatialStorage.Data;
using SpatialStorage.Repository.Context;
using SpatialStorage.Repository.Entities;
using SpatialStorage.Repository.Repository;
using SpatialStorage.Services;

namespace SpatialStorage
{
    public class Startup
    {
        private const string TestDataFile = "Data\\locations.csv";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("Locations");
            services.AddSingleton<ConnectionProvider>(() => connectionString);

            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<SqlServerDbContext>();
            services.AddTransient<IDbContext, SqlServerDbContext>();
            services.AddTransient<ISimpleLocationService, SimpleLocationService>();
            services.AddTransient<IGeographyLocationService, GeographyLocationService>();
            services.AddSingleton(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            Migrate(services).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        /// <summary>
        /// Migrates the database to the latest version and loads the test data for DEBUG builds.
        /// </summary>
        /// <param name="services">The service collection used to build a dependency resolver.</param>
        private async Task Migrate(IServiceCollection services)
        {
            var container = services.BuildServiceProvider();

            using (var context = container.GetService<IDbContext>())
            {
                context.Migrate();

                // Load the test data only for test environments
                #if DEBUG
                    var geographyLocations = services.BuildServiceProvider().GetService<IRepository<GeographyLocation>>();
                    if (!geographyLocations.Any())
                    {
                        var geometryFactory = container.GetService<IGeometryFactory>();
                        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestDataFile);
                        var locations = DataLoader.ReadAll<LocationRecord>(filePath);

                        var locationEntities = locations.Select(location => new GeographyLocation {
                            Address = location.Address,
                            Coordinates = geometryFactory.CreatePoint(new Coordinate(location.Longitude, location.Latitude))
                        });

                        // Dropping index and rebuilding it after the data insert takes less time
                        context.Sql("DROP INDEX IF EXISTS SIX_Locations_Coordinates ON [data].[GeographyLocations];");

                        // Bulk Insert doesn't work with geography data types, therefore a regular insert is used
                        // See https://github.com/borisdj/EFCore.BulkExtensions/issues/88
                        await geographyLocations.AddAsync(locationEntities);
                        await geographyLocations.SaveAsync();

                        // Might take a while to complete...
                        context.Sql("CREATE SPATIAL INDEX SIX_Locations_Coordinates ON [data].[GeographyLocations]([Coordinates]);");
                    }

                    var simpleLocations = services.BuildServiceProvider().GetService<IRepository<SimpleLocation>>();
                    if (!simpleLocations.Any())
                    {
                        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestDataFile);
                        var locations = DataLoader.ReadAll<LocationRecord>(filePath);

                        var locationEntities = locations.Select(location => new SimpleLocation() {
                            Address = location.Address,
                            Latitude = location.Latitude,
                            Longitude = location.Longitude
                        });

                        await simpleLocations.AddBulkAsync(locationEntities);
                    }
                #endif
            }
        }
    }
}