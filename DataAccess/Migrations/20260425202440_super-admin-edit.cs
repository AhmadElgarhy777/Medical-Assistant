using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class superadminedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Pharmacies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Pharmacies",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Pharmacies",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Doctors",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Doctors",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Clinics",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Clinics",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Pharmacies");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Pharmacies");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Clinics");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Clinics");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Pharmacies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
