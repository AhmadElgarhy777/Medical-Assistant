using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editratingcomment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Doctors_DoctorId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Nures_NuresId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_DoctorId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "NuresId",
                table: "Ratings",
                newName: "TargetId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_NuresId",
                table: "Ratings",
                newName: "IX_Ratings_TargetId");

            migrationBuilder.AddColumn<string>(
                name: "TargetRole",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RattingAverage",
                table: "Pharmacies",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TargetId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TargetRole = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_TargetId",
                        column: x => x.TargetId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PatientId",
                table: "Comments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TargetId",
                table: "Comments",
                column: "TargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_TargetId",
                table: "Ratings",
                column: "TargetId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_TargetId",
                table: "Ratings");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropColumn(
                name: "TargetRole",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "RattingAverage",
                table: "Pharmacies");

            migrationBuilder.RenameColumn(
                name: "TargetId",
                table: "Ratings",
                newName: "NuresId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_TargetId",
                table: "Ratings",
                newName: "IX_Ratings_NuresId");

            migrationBuilder.AddColumn<string>(
                name: "DoctorId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_DoctorId",
                table: "Ratings",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Doctors_DoctorId",
                table: "Ratings",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Nures_NuresId",
                table: "Ratings",
                column: "NuresId",
                principalTable: "Nures",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
