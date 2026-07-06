using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class hazomity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AiReports_Doctors_DoctorID",
                table: "AiReports");

            migrationBuilder.DropIndex(
                name: "IX_AiReports_DoctorID",
                table: "AiReports");

            migrationBuilder.DropColumn(
                name: "DoctorID",
                table: "AiReports");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "MediaType",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaUrl",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Governorate = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BanReports",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BannedUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BannedUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BannedUserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BanDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BanCount = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanReports", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Complaints",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaints", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MedicalTests",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TurnaroundHours = table.Column<int>(type: "int", nullable: false),
                    PreparationInstructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequiresFasting = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalTests", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionRequests",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PharmacyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PrescriptionImg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PharmacyNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PrescriptionRequests_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RadiologyScans",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TurnaroundHours = table.Column<int>(type: "int", nullable: false),
                    PreparationInstructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequiresContrast = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadiologyScans", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Labs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LabLicense = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    ReviewsCount = table.Column<int>(type: "int", nullable: false),
                    WorkingHours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupportsHomeCollection = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Labs_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RadiologyCenters",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    ReviewsCount = table.Column<int>(type: "int", nullable: false),
                    WorkingHours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadiologyCenters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RadiologyCenters_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LabTestOffers",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LabId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MedicalTestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTestOffers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LabTestOffers_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LabTestOffers_MedicalTests_MedicalTestId",
                        column: x => x.MedicalTestId,
                        principalTable: "MedicalTests",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LabBookings",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceType = table.Column<int>(type: "int", nullable: false),
                    VisitType = table.Column<int>(type: "int", nullable: false),
                    LabId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RadiologyCenterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AreaId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HomeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledTimeSlot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    HomeCollectionFee = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabBookings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LabBookings_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LabBookings_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LabBookings_RadiologyCenters_RadiologyCenterId",
                        column: x => x.RadiologyCenterId,
                        principalTable: "RadiologyCenters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RadiologyCenterScans",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RadiologyCenterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RadiologyScanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadiologyCenterScans", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RadiologyCenterScans_RadiologyCenters_RadiologyCenterId",
                        column: x => x.RadiologyCenterId,
                        principalTable: "RadiologyCenters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RadiologyCenterScans_RadiologyScans_RadiologyScanId",
                        column: x => x.RadiologyScanId,
                        principalTable: "RadiologyScans",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LabBookingItems",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LabBookingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MedicalTestId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RadiologyScanId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabBookingItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LabBookingItems_LabBookings_LabBookingId",
                        column: x => x.LabBookingId,
                        principalTable: "LabBookings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LabBookingItems_MedicalTests_MedicalTestId",
                        column: x => x.MedicalTestId,
                        principalTable: "MedicalTests",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LabBookingItems_RadiologyScans_RadiologyScanId",
                        column: x => x.RadiologyScanId,
                        principalTable: "RadiologyScans",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LabTestResults",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LabBookingItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ResultFileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultValuesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTestResults", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LabTestResults_LabBookingItems_LabBookingItemId",
                        column: x => x.LabBookingItemId,
                        principalTable: "LabBookingItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabBookingItems_LabBookingId",
                table: "LabBookingItems",
                column: "LabBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_LabBookingItems_MedicalTestId",
                table: "LabBookingItems",
                column: "MedicalTestId");

            migrationBuilder.CreateIndex(
                name: "IX_LabBookingItems_RadiologyScanId",
                table: "LabBookingItems",
                column: "RadiologyScanId");

            migrationBuilder.CreateIndex(
                name: "IX_LabBookings_AreaId",
                table: "LabBookings",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_LabBookings_LabId",
                table: "LabBookings",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_LabBookings_RadiologyCenterId",
                table: "LabBookings",
                column: "RadiologyCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Labs_AreaId",
                table: "Labs",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_LabTestOffers_LabId_MedicalTestId",
                table: "LabTestOffers",
                columns: new[] { "LabId", "MedicalTestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabTestOffers_MedicalTestId",
                table: "LabTestOffers",
                column: "MedicalTestId");

            migrationBuilder.CreateIndex(
                name: "IX_LabTestResults_LabBookingItemId",
                table: "LabTestResults",
                column: "LabBookingItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionRequests_PharmacyId",
                table: "PrescriptionRequests",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_RadiologyCenters_AreaId",
                table: "RadiologyCenters",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_RadiologyCenterScans_RadiologyCenterId_RadiologyScanId",
                table: "RadiologyCenterScans",
                columns: new[] { "RadiologyCenterId", "RadiologyScanId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RadiologyCenterScans_RadiologyScanId",
                table: "RadiologyCenterScans",
                column: "RadiologyScanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BanReports");

            migrationBuilder.DropTable(
                name: "Complaints");

            migrationBuilder.DropTable(
                name: "LabTestOffers");

            migrationBuilder.DropTable(
                name: "LabTestResults");

            migrationBuilder.DropTable(
                name: "PrescriptionRequests");

            migrationBuilder.DropTable(
                name: "RadiologyCenterScans");

            migrationBuilder.DropTable(
                name: "LabBookingItems");

            migrationBuilder.DropTable(
                name: "LabBookings");

            migrationBuilder.DropTable(
                name: "MedicalTests");

            migrationBuilder.DropTable(
                name: "RadiologyScans");

            migrationBuilder.DropTable(
                name: "Labs");

            migrationBuilder.DropTable(
                name: "RadiologyCenters");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MediaUrl",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorID",
                table: "AiReports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AiReports_DoctorID",
                table: "AiReports",
                column: "DoctorID");

            migrationBuilder.AddForeignKey(
                name: "FK_AiReports_Doctors_DoctorID",
                table: "AiReports",
                column: "DoctorID",
                principalTable: "Doctors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
