using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapsVisualisation.Database.Migrations
{
    public partial class AddedLocalImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocalImage",
                table: "Maps",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalImage",
                table: "Maps");
        }
    }
}
