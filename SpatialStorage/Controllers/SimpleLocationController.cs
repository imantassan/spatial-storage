using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpatialStorage.Services;

namespace SpatialStorage.Controllers
{
    [Route("api/simple")]
    [ApiController]
    [Produces("application/json")]
    public class SimpleLocationController : ControllerBase
    {
        private readonly ISimpleLocationService _locationService;

        public SimpleLocationController(ISimpleLocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationDto>>> Get(double lat, double lng, int range = 1000, int maxResults = 1000)
        {
            var stopWatch = Stopwatch.StartNew();
            var results = await _locationService.GetNearbyLocationsAsync(new LocationDto(lat, lng), range, maxResults);
            stopWatch.Stop();

            return Ok(new {Time = stopWatch.Elapsed, Results = results});
        }
    }
}
