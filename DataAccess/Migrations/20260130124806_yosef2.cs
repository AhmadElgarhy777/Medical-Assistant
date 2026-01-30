using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class yosef2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctorAvilableTimes_Doctors_DoctorId",
                table: "doctorAvilableTimes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_doctorAvilableTimes",
                table: "doctorAvilableTimes");

            migrationBuilder.RenameTable(
                name: "doctorAvilableTimes",
                newName: "DoctorAvilableTimes");

            migrationBuilder.RenameIndex(
                name: "IX_doctorAvilableTimes_DoctorId",
                table: "DoctorAvilableTimes",
                newName: "IX_DoctorAvilableTimes_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorAvilableTimes",
                table: "DoctorAvilableTimes",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorAvilableTimes_Doctors_DoctorId",
                table: "DoctorAvilableTimes",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorAvilableTimes_Doctors_DoctorId",
                table: "DoctorAvilableTimes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorAvilableTimes",
                table: "DoctorAvilableTimes");

            migrationBuilder.RenameTable(
                name: "DoctorAvilableTimes",
                newName: "doctorAvilableTimes");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorAvilableTimes_DoctorId",
                table: "doctorAvilableTimes",
                newName: "IX_doctorAvilableTimes_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_doctorAvilableTimes",
                table: "doctorAvilableTimes",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_doctorAvilableTimes_Doctors_DoctorId",
                table: "doctorAvilableTimes",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
