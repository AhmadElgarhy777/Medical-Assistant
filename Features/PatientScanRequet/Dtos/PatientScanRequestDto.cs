using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Dtos
{
    public class PatientScanRequestDto
    {
        public string ScanRequestId { get; set; } = default!;

        public string DoctorId { get; set; } = default!;

        public string DoctorName { get; set; } = default!;

        public AiModelTypeEnum AIModelType { get; set; }

        public ScanRequestStatus Status { get; set; }

        public string? DoctorNote { get; set; }

        public string? RejectReason { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public bool HasUploadedImages { get; set; }

        public int ImagesCount { get; set; }
    }
}
