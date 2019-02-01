using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpatialStorage.Repository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "data");

            migrationBuilder.CreateTable(
                name: "GeographyLocations",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Coordinates = table.Column<IPoint>(nullable: true),
                    Address = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeographyLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimpleLocations",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Address = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleLocations", x => x.Id);
                });

            // Added manually
            migrationBuilder.Sql("CREATE SPATIAL INDEX SIX_Locations_Coordinates ON [data].[GeographyLocations]([Coordinates]);");

            migrationBuilder.CreateIndex(
                name: "IX_SimpleLocations_Latitude_Longitude",
                schema: "data",
                table: "SimpleLocations",
                columns: new[] { "Latitude", "Longitude" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeographyLocations",
                schema: "data");

            migrationBuilder.DropTable(
                name: "SimpleLocations",
                schema: "data");
        }
    }
}
