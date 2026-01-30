using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Yousef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionItems_presciptions_PresciptionId",
                table: "PrescriptionItems");

            migrationBuilder.DropTable(
                name: "presciptions");

            migrationBuilder.AddColumn<string>(
                name: "MedicineName",
                table: "PrescriptionItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsBooked",
                table: "doctorAvilableTimes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ClinicID",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SlotId",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CraetedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppointmentId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ClinicID",
                table: "Appointments",
                column: "ClinicID");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DoctorId",
                table: "Prescriptions",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PatientId",
                table: "Prescriptions",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Clinics_ClinicID",
                table: "Appointments",
                column: "ClinicID",
                principalTable: "Clinics",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionItems_Prescriptions_PresciptionId",
                table: "PrescriptionItems",
                column: "PresciptionId",
                principalTable: "Prescriptions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Clinics_ClinicID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionItems_Prescriptions_PresciptionId",
                table: "PrescriptionItems");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ClinicID",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "MedicineName",
                table: "PrescriptionItems");

            migrationBuilder.DropColumn(
                name: "IsBooked",
                table: "doctorAvilableTimes");

            migrationBuilder.DropColumn(
                name: "ClinicID",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "Appointments");

            migrationBuilder.CreateTable(
                name: "presciptions",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CraetedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_presciptions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_presciptions_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_presciptions_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_presciptions_DoctorId",
                table: "presciptions",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_presciptions_PatientId",
                table: "presciptions",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionItems_presciptions_PresciptionId",
                table: "PrescriptionItems",
                column: "PresciptionId",
                principalTable: "presciptions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
