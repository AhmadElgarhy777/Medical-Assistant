namespace Models
{
    public class LabBookingItem : ModelBase
    {
        public string LabBookingId { get; set; } = null!;
        public LabBooking LabBooking { get; set; } = null!;

        public string? MedicalTestId { get; set; }
        public MedicalTest? MedicalTest { get; set; }

        public string? RadiologyScanId { get; set; }
        public RadiologyScan? RadiologyScan { get; set; }    // ← وده كمان

        public decimal Price { get; set; }

        public LabTestResult? Result { get; set; }
        public RadiologyTestResult? RadiologyResult { get; set; }
    }
}