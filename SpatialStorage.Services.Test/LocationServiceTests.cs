using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SpatialStorage.Services.Test.Helpers;

namespace SpatialStorage.Services.Test
{
    public abstract class LocationServiceTests<TLocation>
        where TLocation : class, new()
    {
        protected ILocationService Service { get; private set; }

        protected TestRepository<TLocation> Repository { get; private set; }

        protected readonly IReadOnlyCollection<LocationDto> TestLocations = new List<LocationDto> {
            new LocationDto(54.683333, 25.283333) {Address = "Vilnius, LT"},
            new LocationDto(54.897222, 23.886111) {Address = "Kaunas, LT"},
            new LocationDto(-27.116667, -109.366667) {Address = "Easter Island, CL"},
            new LocationDto(50.87, -1.58) {Address = "Lyndhurst, UK"},
            new LocationDto(-18.166667, 178.45) {Address = "Suva, FJ"}
        };

        protected readonly LocationDto AnchorPoint = new LocationDto(52.366667, 4.9) {Address = "Amsterdam, NL"};

        protected IReadOnlyCollection<TLocation> StoreLocations { get; private set; }

        protected abstract ILocationService CreateServiceInstance();

        protected abstract TLocation CreateLocation(LocationDto location);

        protected virtual void BeforeSetup() { }

        [SetUp]
        public void Setup()
        {
            BeforeSetup();

            TestLocations.ToList().ForEach(x => x.Distance = x.CalculateDistance(AnchorPoint));
            StoreLocations = TestLocations.Select(CreateLocation).ToList();

            Repository = new TestRepository<TLocation>(StoreLocations);
            Service = CreateServiceInstance();
        }

        [Test]
        public async Task LocationService_GetNearbyLocations_LessThanDistanceToClosestLocationReturnsNoResults()
        {
            // Arrange
            var minDistance = TestLocations.Min(x => x.Distance);

            // Act
            var results = await Service.GetNearbyLocationsAsync(AnchorPoint, (int) minDistance - 1, 1);

            // Assert
            results.Should().BeEmpty();
        }

        [Test]
        public async Task LocationService_GetNearbyLocations_MoreThanMaxDistanceToFarthestLocationReturnsAllResults()
        {
            // Arrange
            var maxDistance = TestLocations.Max(x => x.Distance);

            // Act
            var results = await Service.GetNearbyLocationsAsync(AnchorPoint, (int) maxDistance + 1, TestLocations.Count);

            // Assert
            results.Should().HaveCount(TestLocations.Count);
        }

        [Test]
        public async Task LocationService_GetNearbyLocations_DistanceEqualToABitMoreThanClosestLocationReturnsOnlyOneResult()
        {
            // Arrange
            var minDistance = TestLocations.Min(x => x.Distance);

            // Act
            var results = await Service.GetNearbyLocationsAsync(AnchorPoint, (int) minDistance + 1, TestLocations.Count);

            // Assert
            results.Should().HaveCount(1)
                .And.OnlyContain(x => x.Address == TestLocations.OrderBy(l => l.Distance).First().Address);
        }

        [Test]
        public async Task LocationService_GetNearbyLocations_DistanceEqualToABitLessThanFurthestLocationReturnsAllResultsButOne()
        {
            // Arrange
            var maxDistance = TestLocations.Max(x => x.Distance);

            // Act
            var results = await Service.GetNearbyLocationsAsync(AnchorPoint, (int) maxDistance - 1, TestLocations.Count);

            // Assert
            results.Should().HaveCount(TestLocations.Count - 1)
                .And.NotContain(x => x.Address == TestLocations.OrderByDescending(l => l.Distance).First().Address);
        }

        [Test]
        public async Task LocationService_GetNearbyLocations_RespectsMaxResultsParameter()
        {
            // Arrange
            var maxDistance = TestLocations.Max(x => x.Distance);

            // Act
            var results = await Service.GetNearbyLocationsAsync(AnchorPoint, (int) maxDistance + 1, TestLocations.Count - 1);

            // Assert
            results.Should().HaveCount(TestLocations.Count - 1);
        }

        [Test]
        public async Task LocationService_GetNearbyLocations_ResultsAreOrderedByDistance()
        {
            // Arrange

            // Act
            var results = await Service.GetNearbyLocationsAsync(AnchorPoint, int.MaxValue, int.MaxValue);

            // Assert
            results.Should().BeInAscendingOrder(x => x.Distance);
        }

        [Test]
        public async Task LocationService_GetNearbyLocations_ResultsMatchExpectedOnes()
        {
            // Arrange
            var locationsByAddress = TestLocations.ToDictionary(x => x.Address, x => x);
            var check = new Func<LocationDto, bool>((LocationDto location) =>
            {
                var expected = locationsByAddress[location.Address];

                location.Latitude.Should().Be(expected.Latitude);
                location.Longitude.Should().Be(expected.Longitude);
                // Precision tolerance for 'double'
                location.Distance.Should().BeInRange(expected.Distance - 0.1, expected.Distance + 0.1);

                return true;
            });

            // Act
            var results = await Service.GetNearbyLocationsAsync(AnchorPoint, int.MaxValue, int.MaxValue);

            // Assert
            results.Should().OnlyContain(x => check(x));
        }
    }
}