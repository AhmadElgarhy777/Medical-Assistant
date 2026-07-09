using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ScanRequest : ModelBase
    {
        public string PatientId { get; set; } = null!;
        public ApplicationUser Patient { get; set; } = null!;

        public string DoctorId { get; set; } = null!;
        public ApplicationUser Doctor { get; set; } = null!;

        public AiModelTypeEnum AIModelType { get; set; }

        public ScanRequestStatus Status { get; set; }

        public string? DoctorNote { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? RejectReason { get; set; }
        public string? CancelReason { get; set; }

        public ICollection<RequestedScanImage> Images { get; set; }
            = new HashSet<RequestedScanImage>();

        public AiReport? AiReport { get; set; }
        public string? AiReportId { get; set; }

    }
}
