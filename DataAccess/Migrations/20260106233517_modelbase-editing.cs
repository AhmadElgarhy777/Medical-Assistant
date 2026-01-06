using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class modelbaseediting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpecializationId",
                table: "Specializations",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "RatingId",
                table: "Ratings",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "PrescriptionItemId",
                table: "PrescriptionItems",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "PresciptionId",
                table: "presciptions",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "PatientPhoneId",
                table: "patientPhones",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "DoctorPatientId",
                table: "DoctorPatients",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "DoctorAvilableTimeId",
                table: "doctorAvilableTimes",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ClinicId",
                table: "Clinics",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ClinicPhoneId",
                table: "ClinicPhones",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "Chats",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ChatMessageId",
                table: "ChatMessages",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "Appointments",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "AiReportId",
                table: "AiReports",
                newName: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Specializations",
                newName: "SpecializationId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Ratings",
                newName: "RatingId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "PrescriptionItems",
                newName: "PrescriptionItemId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "presciptions",
                newName: "PresciptionId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "patientPhones",
                newName: "PatientPhoneId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "DoctorPatients",
                newName: "DoctorPatientId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "doctorAvilableTimes",
                newName: "DoctorAvilableTimeId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Clinics",
                newName: "ClinicId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ClinicPhones",
                newName: "ClinicPhoneId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Chats",
                newName: "ChatId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ChatMessages",
                newName: "ChatMessageId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Appointments",
                newName: "AppointmentId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "AiReports",
                newName: "AiReportId");
        }
    }
}
