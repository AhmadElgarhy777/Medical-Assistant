using System;
using Models.Enums;

namespace Models
{
    public class PatientMedicalScan : ModelBase
    {
        public string PatientId { get; set; } = null!;
        public Patient Patient { get; set; } = null!;

        public string DoctorId { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;

        public AiModelTypeEnum ModelType { get; set; }

        public string ImagePath { get; set; } = null!;

        public MedicalScanStatusEnum Status { get; set; } = MedicalScanStatusEnum.PendingDoctorReview;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ApprovedAt { get; set; }

        public string? DoctorNote { get; set; }

        public string? AiReportId { get; set; }
        public AiReport? AiReport { get; set; }
    }
}
