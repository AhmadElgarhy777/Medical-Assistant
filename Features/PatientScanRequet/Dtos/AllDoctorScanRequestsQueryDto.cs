using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Dtos
{
    public class AllDoctorScanRequestsQueryDto
    {
        public string ScanRequestId { get; set; } = default!;

        public string PatientId { get; set; } = default!;

        public string PatientName { get; set; } = default!;

        public AiModelTypeEnum AIModelType { get; set; }

        public ScanRequestStatus Status { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public int ImagesCount { get; set; }

        public bool HasReport { get; set; }
    }
}
