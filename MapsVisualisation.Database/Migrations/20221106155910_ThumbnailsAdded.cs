using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapsVisualisation.Database.Migrations
{
    public partial class ThumbnailsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Maps",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Maps");
        }
    }
}
