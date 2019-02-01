using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NSubstitute;
using SpatialStorage.Repository.Entities;

namespace SpatialStorage.Services.Test
{
    public class GeographyLocationServiceTests : LocationServiceTests<GeographyLocation>
    {
        private IGeometryFactory _geometryFactory;

        protected override void BeforeSetup()
        {
            _geometryFactory = Substitute.For<IGeometryFactory>();

            _geometryFactory.CreatePoint(Arg.Any<Coordinate>())
                .Returns(c => new Point(c.Arg<Coordinate>().X, c.Arg<Coordinate>().Y));
        }

        protected override ILocationService CreateServiceInstance() =>
            new GeographyLocationService(Repository, _geometryFactory);

        protected override GeographyLocation CreateLocation(LocationDto location)
        {
            var point = Substitute.For<IPoint>();
            point.Distance(Arg.Is<IPoint>(p => p.X == AnchorPoint.Longitude && p.Y == AnchorPoint.Latitude))
                .Returns(location.Distance);
            point.X.Returns(location.Longitude);
            point.Y.Returns(location.Latitude);

            return new GeographyLocation {
                Address = location.Address,
                Coordinates = point,
            };
        }
    }
}