namespace SpatialStorage.Data
{
    /// <summary>
    /// Represents a single location record within a data set.
    /// </summary>
    public class LocationRecord
    {
        /// <summary>
        /// Gets or sets the address or title of the location.
        /// </summary>
        /// <value>The address.</value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the location.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the location.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set; }
    }
}