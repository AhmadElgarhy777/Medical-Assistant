using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editpharmacyProducttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "PharmacyProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "PharmacyProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PharmacyId",
                table: "PharmacyProducts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "Inventories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinQuantity",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PharmacyRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PharmacyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmacyRatings_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyProducts_PharmacyId",
                table: "PharmacyProducts",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyRatings_PharmacyId",
                table: "PharmacyRatings",
                column: "PharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PharmacyProducts_Pharmacies_PharmacyId",
                table: "PharmacyProducts",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PharmacyProducts_Pharmacies_PharmacyId",
                table: "PharmacyProducts");

            migrationBuilder.DropTable(
                name: "PharmacyRatings");

            migrationBuilder.DropIndex(
                name: "IX_PharmacyProducts_PharmacyId",
                table: "PharmacyProducts");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "PharmacyProducts");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "PharmacyProducts");

            migrationBuilder.DropColumn(
                name: "PharmacyId",
                table: "PharmacyProducts");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "MinQuantity",
                table: "Inventories");
        }
    }
}
