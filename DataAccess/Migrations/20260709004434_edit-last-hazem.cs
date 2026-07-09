using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editlasthazem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Specializations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "scanRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "requestedScanImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "RefreshTokens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "RadiologyScans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "RadiologyCenterScans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "RadiologyCenters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "RadiologyCenters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "RadiologyCenters",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "RadiologyCenters",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "RadiologyCenters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "PrescriptionRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "PrescriptionItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "PharmacyProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Pharmacies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "patientPhones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "InventoryId",
                table: "OrderItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MedicineName",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "NursingServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "NurseServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Nures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "MedicalTests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "LabTestResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "LabTestOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Labs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "LabBookings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "LabBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CollectorId",
                table: "LabBookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "LabBookingItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "DoctorPatients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "DoctorAvilableTimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Conversations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "conversationParticipants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Complaints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Clinics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "ClinicPhones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Areas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "AiReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "AiReportImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Admins",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LabSchedules",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LabId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    FromTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ToTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BanCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabSchedules", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LabSchedules_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RadiologySchedules",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RadiologyCenterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    FromTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ToTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BanCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadiologySchedules", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RadiologySchedules_RadiologyCenters_RadiologyCenterId",
                        column: x => x.RadiologyCenterId,
                        principalTable: "RadiologyCenters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RadiologyTestResults",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LabBookingItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReportFileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagesUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BanCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadiologyTestResults", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RadiologyTestResults_LabBookingItems_LabBookingItemId",
                        column: x => x.LabBookingItemId,
                        principalTable: "LabBookingItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabBookings_PatientId",
                table: "LabBookings",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_LabSchedules_LabId",
                table: "LabSchedules",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_RadiologySchedules_RadiologyCenterId",
                table: "RadiologySchedules",
                column: "RadiologyCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_RadiologyTestResults_LabBookingItemId",
                table: "RadiologyTestResults",
                column: "LabBookingItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LabBookings_Patients_PatientId",
                table: "LabBookings",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabBookings_Patients_PatientId",
                table: "LabBookings");

            migrationBuilder.DropTable(
                name: "LabSchedules");

            migrationBuilder.DropTable(
                name: "RadiologySchedules");

            migrationBuilder.DropTable(
                name: "RadiologyTestResults");

            migrationBuilder.DropIndex(
                name: "IX_LabBookings_PatientId",
                table: "LabBookings");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "scanRequests");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "requestedScanImages");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "RadiologyScans");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "RadiologyCenterScans");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "RadiologyCenters");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "RadiologyCenters");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "RadiologyCenters");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "RadiologyCenters");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RadiologyCenters");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "PrescriptionRequests");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "PrescriptionItems");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "PharmacyProducts");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Pharmacies");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "patientPhones");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "MedicineName",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "NursingServices");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "NurseServices");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Nures");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "LabTestResults");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "LabTestOffers");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Labs");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "LabBookings");

            migrationBuilder.DropColumn(
                name: "CollectorId",
                table: "LabBookings");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "LabBookingItems");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "DoctorPatients");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "DoctorAvilableTimes");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "conversationParticipants");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Clinics");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "ClinicPhones");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "AiReports");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "AiReportImages");

            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Admins");

            migrationBuilder.AlterColumn<string>(
                name: "InventoryId",
                table: "OrderItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "LabBookings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
