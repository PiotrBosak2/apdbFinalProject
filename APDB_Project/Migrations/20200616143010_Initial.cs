using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APDB_Project.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Building2",
                columns: table => new
                {
                    IdBuilding = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetNumber = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Building2", x => x.IdBuilding);
                });

            migrationBuilder.CreateTable(
                name: "Client2",
                columns: table => new
                {
                    IdClient = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client2", x => x.IdClient);
                });

            migrationBuilder.CreateTable(
                name: "Campaign2",
                columns: table => new
                {
                    IdCampaign = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdClient = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PricePerSquareMeter = table.Column<double>(type: "float", nullable: false),
                    FromIdBuilding = table.Column<int>(type: "int", nullable: true),
                    ToIdBuilding = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaign2", x => x.IdCampaign);
                    table.ForeignKey(
                        name: "FK_Campaign2_Building2_FromIdBuilding",
                        column: x => x.FromIdBuilding,
                        principalTable: "Building2",
                        principalColumn: "IdBuilding");
                    table.ForeignKey(
                        name: "FK_Campaign2_Building2_ToIdBuilding",
                        column: x => x.ToIdBuilding,
                        principalTable: "Building2",
                        principalColumn: "IdBuilding");
                    table.ForeignKey(
                        name: "FK_Campaign2_Client2_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Client2",
                        principalColumn: "IdClient");
                });

            migrationBuilder.CreateTable(
                name: "Banner2",
                columns: table => new
                {
                    IdBanner = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    IdCampaign = table.Column<int>(type: "int", nullable: false),
                    Area = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banner2", x => x.IdBanner);
                    table.ForeignKey(
                        name: "FK_Banner2_Campaign2_IdCampaign",
                        column: x => x.IdCampaign,
                        principalTable: "Campaign2",
                        principalColumn: "IdCampaign",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Banner2_IdCampaign",
                table: "Banner2",
                column: "IdCampaign");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign2_FromIdBuilding",
                table: "Campaign2",
                column: "FromIdBuilding");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign2_IdClient",
                table: "Campaign2",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign2_ToIdBuilding",
                table: "Campaign2",
                column: "ToIdBuilding");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banner2");

            migrationBuilder.DropTable(
                name: "Campaign2");

            migrationBuilder.DropTable(
                name: "Building2");

            migrationBuilder.DropTable(
                name: "Client2");
        }
    }
}
