using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models.Enums;

namespace Models
{
    public class RadiologyTestResult : ModelBase
    {
        public string LabBookingItemId { get; set; } = null!;
        public LabBookingItem LabBookingItem { get; set; } = null!;

        public ResultStatusEnum Status { get; set; } = ResultStatusEnum.NotReady;
        public string? ReportFileUrl { get; set; }
        public string? ImagesUrls { get; set; } // comma-separated or json string
        public string? DoctorNotes { get; set; }
        public DateTime? ReportedAt { get; set; }
    }
}
