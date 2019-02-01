using System.Diagnostics;

namespace SpatialStorage.Services
{
    /// <summary>
    /// DTO used to transfer data between services.
    /// </summary>
    public class LocationDto
    {
        /// <summary>
        /// Gets or sets the latitude of the location.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the location.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set;  }

        /// <summary>
        /// Gets or sets the address as the title of the location.
        /// </summary>
        /// <value>The address.</value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the distance from a specific service defined point.
        /// </summary>
        /// <value>The distance.</value>
        public double Distance { get; set; }

        internal LocationDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationDto"/> class.
        /// </summary>
        /// <param name="latitude">The latitude of the location.</param>
        /// <param name="longitude">The longitude of the location.</param>
        public LocationDto(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Address} ({Latitude},{Longitude}){(Distance != default(double) ? " - " + Distance : "")}";
        }
    }
}