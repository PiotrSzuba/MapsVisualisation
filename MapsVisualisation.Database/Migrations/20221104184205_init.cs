using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapsVisualisation.Database.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RegionName1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionName3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionIdentity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NWLat = table.Column<double>(type: "float", nullable: false),
                    NWLong = table.Column<double>(type: "float", nullable: false),
                    NELat = table.Column<double>(type: "float", nullable: false),
                    NELong = table.Column<double>(type: "float", nullable: false),
                    SELat = table.Column<double>(type: "float", nullable: false),
                    SELong = table.Column<double>(type: "float", nullable: false),
                    SWLat = table.Column<double>(type: "float", nullable: false),
                    SWLong = table.Column<double>(type: "float", nullable: false),
                    RegionType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    PublishYear = table.Column<int>(type: "int", nullable: false),
                    Dpi = table.Column<int>(type: "int", nullable: false),
                    CollectionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maps_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Maps_RegionId",
                table: "Maps",
                column: "RegionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
