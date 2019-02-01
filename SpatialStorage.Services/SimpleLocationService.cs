using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpatialStorage.Repository.Entities;
using SpatialStorage.Repository.Repository;

namespace SpatialStorage.Services
{
    /// <summary>
    /// Service used to retrieve locations using simple data types in the database (no geography data types).
    /// Implements the <see cref="ISimpleLocationService" />
    /// </summary>
    /// <seealso cref="ISimpleLocationService" />
    public class SimpleLocationService : ISimpleLocationService
    {
        private readonly IReadOnlyRepository<SimpleLocation> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleLocationService"/> class.
        /// </summary>
        /// <param name="repository">The repository to use for retrieving locations from storage.</param>
        public SimpleLocationService(IReadOnlyRepository<SimpleLocation> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<LocationDto>> GetNearbyLocationsAsync(LocationDto location, int range,
            int maxResults = 0)
        {
            var query = from dbLoc in _repository
                let radiansLat1 = Math.PI * dbLoc.Latitude / 180
                let radiansLat2 = Math.PI * location.Latitude / 180
                let theta = dbLoc.Longitude - location.Longitude
                let radiansTheta = Math.PI * theta / 180
                let dist = Math.Acos(Math.Sin(radiansLat1) * Math.Sin(radiansLat2) +
                                     Math.Cos(radiansLat1) * Math.Cos(radiansLat2) * Math.Cos(radiansTheta)) *
                                     180 * 60 * 1.1515 * 1609.344 / Math.PI // Radius of the earth ~6371km
                where dist <= range
                orderby dist
                select new LocationDto {
                    Latitude = dbLoc.Latitude,
                    Longitude = dbLoc.Longitude,
                    Address = dbLoc.Address,
                    Distance = dist
                };

            if (maxResults > 0)
            {
                query = query.Take(maxResults);
            }

            return await query.ToListAsync();
        }
    }
}