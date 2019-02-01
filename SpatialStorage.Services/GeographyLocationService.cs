using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore;
using SpatialStorage.Repository.Entities;
using SpatialStorage.Repository.Repository;

namespace SpatialStorage.Services
{
    /// <inheritdoc />
    /// <summary>
    /// Class used to retrieve locations using geography data types of the database.
    /// Implements the <see cref="T:SpatialStorage.Services.IGeographyLocationService" />
    /// </summary>
    /// <seealso cref="T:SpatialStorage.Services.IGeographyLocationService" />
    public class GeographyLocationService : IGeographyLocationService
    {
        private readonly IReadOnlyRepository<GeographyLocation> _repository;
        private readonly IGeometryFactory _geometryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographyLocationService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="geometryFactory">The geometry factory.</param>
        public GeographyLocationService(IReadOnlyRepository<GeographyLocation> repository, IGeometryFactory geometryFactory)
        {
            _repository = repository;
            _geometryFactory = geometryFactory;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<LocationDto>> GetNearbyLocationsAsync(LocationDto location, int range, int maxResults)
        {
            var point = _geometryFactory.CreatePoint(new Coordinate(location.Longitude, location.Latitude));
            var query = from dbLoc in _repository
                let distance = dbLoc.Coordinates.Distance(point)
                where distance <= range
                orderby distance
                select new LocationDto {
                    Latitude = dbLoc.Coordinates.Y,
                    Longitude = dbLoc.Coordinates.X,
                    Distance = distance,
                    Address = dbLoc.Address
                };

            if (maxResults > 0)
            {
                query = query.Take(maxResults);
            }

            return await query.ToListAsync();
        }
    }
}