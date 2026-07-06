using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AiReport : ModelBase
    {
        public string Diagnosis { get; set; } = null!;

        public double Confidence { get; set; }
            
        public AiModelTypeEnum ModelType { get; set; } 

        public string? RawApiResponse { get; set; }

        public string? DoctorNote { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string PatientId { get; set; } = null!;

        public Patient Patient { get; set; } = null!;

        public string DoctorId { get; set; } = null!;

        public Doctor Doctor { get; set; } = null!;

        public ICollection<AiReportImage> Images { get; set; }
            = new List<AiReportImage>();
    }

    public class AiReportImage : ModelBase
    {
        //public string FileName { get; set; } = null!;

        public string ImagePath { get; set; } = null!;

        public string ContentType { get; set; } = null!;

        //public long FileSize { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;

        public string AiReportId { get; set; } = null!; 

        public AiReport AiReport { get; set; } = null!;
    }



    public enum AiModelTypeEnum
    {
        BrainTumorDetection=1,
        SkinCancerClassification=2,
        ChestRayClassifcation=3,
        CBCBloodTest =4

    }
}
