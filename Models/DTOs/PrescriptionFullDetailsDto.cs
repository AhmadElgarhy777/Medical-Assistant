namespace Models.DTOs
{
    public class PrescriptionFullDetailsDto
    {
        public string PrescriptionId { get; set; } 
        public string Diagnosis { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string DoctorName { get; set; } = null!;
        public string PatientName { get; set; } = null!;
        public List<MedicineItemDto> Medicines { get; set; } = new();
    }

    public class MedicineItemDto
    {
        public string DrugName { get; set; } = null!;
        public string Dosage { get; set; } = null!;
    }
}