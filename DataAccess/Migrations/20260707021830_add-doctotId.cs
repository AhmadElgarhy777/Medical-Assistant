using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class adddoctotId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
            name: "DoctorId",
           table: "AiReports",
           type: "nvarchar(450)",
           nullable: false,
           defaultValue: "");

            migrationBuilder.CreateIndex(
    name: "IX_AiReports_DoctorId",
    table: "AiReports",
    column: "DoctorId");

            migrationBuilder.AddForeignKey(
    name: "FK_AiReports_Doctors_DoctorId",
    table: "AiReports",
    column: "DoctorId",
    principalTable: "Doctors",
    principalColumn: "ID",
    onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
name: "FK_AiReports_Doctors_DoctorId",
table: "AiReports");

            migrationBuilder.DropIndex(
                name: "IX_AiReports_DoctorId",
                table: "AiReports");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "AiReports");
        }
    }
}
