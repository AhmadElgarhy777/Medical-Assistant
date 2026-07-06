using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class aiServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AiReportOutput",
                table: "AiReports");

            migrationBuilder.DropColumn(
                name: "Img",
                table: "AiReports");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorNote",
                table: "AiReports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<double>(
                name: "Confidence",
                table: "AiReports",
                type: "float(5)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Diagnosis",
                table: "AiReports",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");


            migrationBuilder.AddColumn<string>(
                name: "ModelType",
                table: "AiReports",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RawApiResponse",
                table: "AiReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AiReportImages",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AiReportId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiReportImages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AiReportImages_AiReports_AiReportId",
                        column: x => x.AiReportId,
                        principalTable: "AiReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

           

            migrationBuilder.CreateIndex(
                name: "IX_AiReportImages_AiReportId",
                table: "AiReportImages",
                column: "AiReportId");

          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropTable(
                name: "AiReportImages");

            

            migrationBuilder.DropColumn(
                name: "Confidence",
                table: "AiReports");

            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "AiReports");

           

            migrationBuilder.DropColumn(
                name: "ModelType",
                table: "AiReports");

            migrationBuilder.DropColumn(
                name: "RawApiResponse",
                table: "AiReports");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorNote",
                table: "AiReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AiReportOutput",
                table: "AiReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "AiReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
