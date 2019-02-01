using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SpatialStorage.Repository.Entities
{
    /// <summary>
    /// Represents a single entry in the database using simple data types (<c>double</c> instead of geography)
    /// </summary>
    [Table("SimpleLocations", Schema = "data")]
    public class SimpleLocation
    {
        /// <summary>
        /// Gets or sets the ID of the entry.
        /// </summary>
        /// <value>The ID of the entry.</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the location.
        /// </summary>
        /// <value>The latitude of the location.</value>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the location.
        /// </summary>
        /// <value>The longitude of the location.</value>
        public double Longitude { get; set; }

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
            modelBuilder.Entity<SimpleLocation>(entity =>
            {
                entity.HasIndex(x => new {x.Latitude, x.Longitude});
            });
        }
    }
}