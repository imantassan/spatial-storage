using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore;

namespace SpatialStorage.Repository.Entities
{
    /// <summary>
    /// Represents a single location entry in the database using geography data types
    /// </summary>
    [Table("GeographyLocations", Schema = "data")]
    public class GeographyLocation
    {
        /// <summary>
        /// Gets or sets the ID of the entry.
        /// </summary>
        /// <value>The ID of the entry.</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the coordinates of the location.
        /// </summary>
        /// <value>The coordinates of the location.</value>
        /// <seealso cref="IGeometryFactory">For creating instances of type <see cref="IPoint"/>.</seealso>
        public IPoint Coordinates { get; set; }

        /// <summary>
        /// Gets or sets the address as the title of the location.
        /// </summary>
        /// <value>The address of the location.</value>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Registers the entity with the given <paramref name="modelBuilder"/>.
        /// </summary>
        /// <param name="modelBuilder">The model builder to register the entity with.</param>
        public static void Register(ModelBuilder modelBuilder)
        {
            // EF Core as of 2.2 still doesn't support spatial indexes, migrations require manual adjustment
            // See https://github.com/aspnet/EntityFrameworkCore/issues/12538
            modelBuilder.Entity<GeographyLocation>(entity =>
            {
                entity.HasIndex(x => new {x.Coordinates});
            });
        }
    }
}