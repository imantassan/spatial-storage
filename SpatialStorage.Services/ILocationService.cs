using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpatialStorage.Services
{
    /// <summary>
    /// Contains methods needed to retrieve certain locations.
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        /// Gets the nearby locations around the specific <paramref name="location"/> point.
        /// </summary>
        /// <param name="location">The point to get the locations around.</param>
        /// <param name="range">The range of location lookup around the point in meters.</param>
        /// <param name="maxResults">The maximum number of results to return.</param>
        /// <returns>A collection of locations matching the search criteria.</returns>
        Task<IEnumerable<LocationDto>> GetNearbyLocationsAsync(LocationDto location, int range, int maxResults);
    }
}