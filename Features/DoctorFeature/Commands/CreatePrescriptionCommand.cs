using MediatR;
using System.Collections.Generic;

namespace Features.DoctorFeature.Commands
{
    public record CreatePrescriptionCommand(
        string AppointmentId,
        string PatientId,
        string DoctorId,
        string Diagnosis,
        List<MedicineInput> Medicines
    ) : IRequest<bool>;

    public class MedicineInput
    {
        public string Name { get; set; } = null!; // ده اللي هيروح لـ DrugName
        public string Dosage { get; set; } = null!; // ده اللي هيروح لـ Dos
    }
}