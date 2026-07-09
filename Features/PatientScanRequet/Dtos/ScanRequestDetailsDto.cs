using Models;
using Models.DTOs;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Dtos
{
    public class ScanRequestDetailsDto
    {
        public string ScanRequestId { get; set; } = default!;

        public string PatientId { get; set; } = default!;

        public string PatientName { get; set; } = default!;

        public AiModelTypeEnum AIModelType { get; set; }

        public ScanRequestStatus Status { get; set; }

        public string? DoctorNote { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public AiReportForScaanDto? AiReport { get; set; }

        public List<ScanRequestImageDto> Images { get; set; } = [];

    }
    public class AiReportForScaanDto
    {
        public string ReportId { get; set; } = default!;

        public string Diagnosis { get; set; } = default!;

        public double Confidence { get; set; }

        public string? DoctorNote { get; set; }

        public AiModelTypeEnum ModelType { get; set; }
    }
    public class ScanRequestImageDto
    {
        public string ImageId { get; set; } = default!;

        public string ImagePath { get; set; } = default!;
    }
}
