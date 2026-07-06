using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AiReportDTO
    {
        public ReportOutPutDto AiReportOutput { get; set; } = null!;
        public string DoctorNote { get; set; } = null!;
        public string CreatedAt { get; set; } = null!;
        public string DoctorName { get; set; }=null!;
        public string? DoctorSpeclization { get; set; }

        public List<ReportimagesDto> Images { get; set; }= new List<ReportimagesDto>();

    }

    public class ReportOutPutDto
    {
        public string Diagnosis { get; set; }=null!;
        public double confidence { get; set; }
    }
    public class ReportimagesDto
    {
        public string ImagePath { get; set; } = null!;

        public string ContentType { get; set; } = null!;

        public string UploadedAt { get; set; }=null!;

    }
}
