using SpatialStorage.Repository.Entities;

namespace SpatialStorage.Services.Test
{
    public class SimpleLocationServiceTests : LocationServiceTests<SimpleLocation>
    {
        private int _index;

        protected override ILocationService CreateServiceInstance() => new SimpleLocationService(Repository);

        protected override SimpleLocation CreateLocation(LocationDto location)
            => new SimpleLocation {
                Id = _index++,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Address = location.Address
            };
    }
}