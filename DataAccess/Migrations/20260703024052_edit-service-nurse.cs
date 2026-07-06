using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editservicenurse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PricePerDay",
                table: "Nures",
                newName: "PricePerHours");

            migrationBuilder.AddColumn<int>(
                name: "NurseSpecialty",
                table: "Nures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WorkAt",
                table: "Nures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressNote",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Bookings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Bookings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "bookDetails",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NursingServices",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NursingServices", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NurseServices",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NurseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServiceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NurseServices", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NurseServices_Nures_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NurseServices_NursingServices_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "NursingServices",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NurseServices_NurseId",
                table: "NurseServices",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_NurseServices_ServiceId",
                table: "NurseServices",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NurseServices");

            migrationBuilder.DropTable(
                name: "NursingServices");

            migrationBuilder.DropColumn(
                name: "NurseSpecialty",
                table: "Nures");

            migrationBuilder.DropColumn(
                name: "WorkAt",
                table: "Nures");

            migrationBuilder.DropColumn(
                name: "AddressNote",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "bookDetails",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "PricePerHours",
                table: "Nures",
                newName: "PricePerDay");
        }
    }
}
