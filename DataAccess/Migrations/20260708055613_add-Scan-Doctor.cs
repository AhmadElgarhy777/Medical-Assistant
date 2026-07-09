using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addScanDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientMedicalScans");

            migrationBuilder.AddColumn<string>(
                name: "ScanRequestId",
                table: "AiReports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "scanRequests",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AIModelType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scanRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_scanRequests_AspNetUsers_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_scanRequests_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "requestedScanImages",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ScanRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requestedScanImages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_requestedScanImages_scanRequests_ScanRequestId",
                        column: x => x.ScanRequestId,
                        principalTable: "scanRequests",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AiReports_ScanRequestId",
                table: "AiReports",
                column: "ScanRequestId",
                unique: true,
                filter: "[ScanRequestId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_requestedScanImages_ScanRequestId",
                table: "requestedScanImages",
                column: "ScanRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_scanRequests_DoctorId",
                table: "scanRequests",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_scanRequests_PatientId",
                table: "scanRequests",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_AiReports_scanRequests_ScanRequestId",
                table: "AiReports",
                column: "ScanRequestId",
                principalTable: "scanRequests",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AiReports_scanRequests_ScanRequestId",
                table: "AiReports");

            migrationBuilder.DropTable(
                name: "requestedScanImages");

            migrationBuilder.DropTable(
                name: "scanRequests");

            migrationBuilder.DropIndex(
                name: "IX_AiReports_ScanRequestId",
                table: "AiReports");

            migrationBuilder.DropColumn(
                name: "ScanRequestId",
                table: "AiReports");

            migrationBuilder.CreateTable(
                name: "PatientMedicalScans",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AiReportId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DoctorNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModelType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientMedicalScans", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientMedicalScans_AiReports_AiReportId",
                        column: x => x.AiReportId,
                        principalTable: "AiReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientMedicalScans_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientMedicalScans_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedicalScans_AiReportId",
                table: "PatientMedicalScans",
                column: "AiReportId",
                unique: true,
                filter: "[AiReportId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedicalScans_DoctorId",
                table: "PatientMedicalScans",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedicalScans_PatientId",
                table: "PatientMedicalScans",
                column: "PatientId");
        }
    }
}
