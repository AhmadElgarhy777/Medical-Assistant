using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ConfigUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Patients",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "NuresId",
                table: "Nures",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Doctors",
                newName: "ID");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerDay",
                table: "Nures",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Patients",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Nures",
                newName: "NuresId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Doctors",
                newName: "DoctorId");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerDay",
                table: "Nures",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
